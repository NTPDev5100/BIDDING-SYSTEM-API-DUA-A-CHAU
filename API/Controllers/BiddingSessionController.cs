using AppDbContext;
using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Hangfire;
using Interface.DbContext;
using Interface.Services;
using Interface.Services.Catalogue;
using Interface.Services.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using Service.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;
using static Utilities.CoreContants;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý phiên đấu thầu
    /// </summary>
    [Route("api/biddingsession")]
    [ApiController]
    [Description("Quản lý phiên đấu thầu")]
    [Authorize]
    public class BiddingSessionController : BaseController<tbl_BiddingSessions, BiddingSessionModel, BiddingSessionCreate, BiddingSessionUpdate, BiddingSessionsSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductsService productsService;
        private readonly IBiddingService biddingService;
        private readonly IBiddingSessionService biddingSessionService;
        private readonly IBiddingTicketService biddingTicketService;
        private readonly IProviderService providerService;
        private readonly INotificationService notificationService;
        private readonly IAppDbContext appDbContext;
        public BiddingSessionController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_BiddingSessions, BiddingSessionModel, BiddingSessionCreate, BiddingSessionUpdate, BiddingSessionsSearch>> logger,
            IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            _httpContextAccessor = httpContextAccessor;
            biddingService = serviceProvider.GetRequiredService<IBiddingService>();
            productsService = serviceProvider.GetRequiredService<IProductsService>();
            domainService = serviceProvider.GetRequiredService<IBiddingSessionService>();
            biddingSessionService = serviceProvider.GetRequiredService<IBiddingSessionService>();
            biddingTicketService = serviceProvider.GetRequiredService<IBiddingTicketService>();
            providerService = serviceProvider.GetRequiredService<IProviderService>();
            notificationService = serviceProvider.GetRequiredService<INotificationService>();
            this.appDbContext = serviceProvider.GetRequiredService<IAppDbContext>();
        }
        /// <summary>
        /// Thêm mới phiên đấu thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới phiên đấu thầu")]
        public override async Task<AppDomainResult> AddItem([FromBody] BiddingSessionCreate itemModel)
        {
            using (var tran = await appDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!ModelState.IsValid)
                        throw new AppException(ModelState.GetErrorMessage());
                    var item = mapper.Map<tbl_BiddingSessions>(itemModel);
                    if (item == null)
                        throw new AppException("Item không tồn tại!");
                    if (itemModel.StartDate < Timestamp.UtcNow())
                        throw new AppException("Thời gian diễn ra không được nhỏ hơn thời gian hiện tại!");
                    if (itemModel.EndDate < Timestamp.UtcNow())
                        throw new AppException("Thời gian kết thúc không được nhỏ hơn thời gian hiện tại!");
                    if (itemModel.EndDate < itemModel.StartDate)
                        throw new AppException("Thời gian kết thúc không được nhỏ hơn thời gian diễn ra!");
                    if (itemModel.MaximumQuantity < itemModel.MinimumQuantity)
                        throw new AppException("Số lượng tối đa không được nhỏ hơn số lương tối thiểu!");
                    var bidding = await biddingService.GetByIdAsync(itemModel.BiddingId.Value);
                    if (bidding == null)
                        throw new AppException("Gói thầu không tồn tại!");
                    var product = await productsService.GetByIdAsync(bidding.ProductId.Value);
                    if (product == null)
                        throw new AppException("Sản phẩm trong gói thầu không tồn tại!");
                    string code = "ACP_" + product.Code + "_" + Timestamp.DateTimeFormatISO(DateTime.Now) + "_" + WeekOrderInYear.GetIso8601WeekOfYear(DateTime.Now);

                    item.Id = Guid.NewGuid();
                    item.Code = code;
                    item.BiddingId = bidding.Id;
                    //Trạng thái gói thầu = 0, chưa được diễn ra
                    item.Status = (int)StatusBiddingSession.ChuaDienRa;
                    await Task.Run(() =>
                    {
                        // =>>>> Các bước xử lý lấy timeout của phiên thầu đưa vào schedule job hangfire
                        // Job bắt sự kiện diễn ra của phiên thầu
                        var timeOutStartBidSession = (itemModel.StartDate - Timestamp.UtcNow());
                        TimeSpan secondTimeOutStart = TimeSpan.FromSeconds(timeOutStartBidSession.Value);
                        var jobBidSessionStartId = BackgroundJob.Schedule(() => biddingSessionService.ChangeStatusBiddingSessionStart(item), secondTimeOutStart);
                        // Job bắt sự kiện kết thúc của phiên thầu
                        var timeOutEndBidSession = (itemModel.EndDate - Timestamp.UtcNow());
                        TimeSpan secondTimeOutEnd = TimeSpan.FromSeconds(timeOutEndBidSession.Value);
                        var jobBidSessionEndId = BackgroundJob.Schedule(() => biddingSessionService.ChangeStatusBiddingSessionEnd(item), secondTimeOutEnd);
                        // Lưu 2 jobid vào db
                        var jobIdList = new ObjectJobId { JobId1 = jobBidSessionStartId, JobId2 = jobBidSessionEndId };
                        item.JobHangFireId = JsonConvert.SerializeObject(jobIdList);
                    });
                    await this.domainService.CreateAsync(item);


                    ////Gửi thông báo cho ncc
                    //#region Các bước Thông báo cho người dùng nhóm khách hàng
                    //var provider = await providerService.GetAsync(x => x.Active == true && x.Deleted == false);
                    //if (provider != null || provider.Any())
                    //{
                    //    string title = "Phiên thầu mới";
                    //    //Nội dung thông báo
                    //    StringBuilder contentNotification = new StringBuilder();
                    //    contentNotification.AppendLine("Thông tin phiên thầu mới");
                    //    contentNotification.AppendLine($"Tên phiên thầu: {item.Name}");
                    //    contentNotification.AppendLine($"Tên sản phẩm: {product.Name}");
                    //    contentNotification.AppendLine($"Mô tả phiên thầu: {item.Description}");
                    //    contentNotification.AppendLine($"Thời gian bắt đầu: {Timestamp.UnixTimestampToDateTime(item.StartDate.Value).ToString("dd-MM-yyyy HH:mm:ss")}");
                    //    contentNotification.AppendLine($"Thời gian kết thúc: {Timestamp.UnixTimestampToDateTime(item.EndDate.Value).ToString("dd-MM-yyyy HH:mm:ss")}");
                    //    await notificationService.CreateAsync(provider.Select(i => new tbl_Notification()
                    //    {
                    //        UserId = i.Id,
                    //        Title = title,
                    //        Content = contentNotification.ToString(),
                    //        IsSeen = false,
                    //        Created = Timestamp.UtcNow(),
                    //        IsType = (int)TypeNotification.PhienThauBatDau
                    //    }).ToList());


                    //    // => 2:Gửi thông báo qua Onesignal
                    //    List<string> oneSignal_DeviceIds = provider.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();        
                    //    OneSignalPush.OneSignalMobilePushNotifications(title, contentNotification.ToString(), oneSignal_DeviceIds, string.Empty);
                    //    #endregion
                    //}
                    await tran.CommitAsync();
                }
                //Nếu trong quá trình Create thất bại thì rollback 
                catch (AppException e)
                {
                    await tran.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới phiên thành công!",
                Success = true
            };
        }


        /// <summary>
        /// Cập nhật thông tin phiêu đấu thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật phiên đấu thầu")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] BiddingSessionUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_BiddingSessions>(itemModel);
            if (item == null)
                throw new KeyNotFoundException("Item không tồn tại!");
            var biddingSession = await domainService.GetByIdAsync(itemModel.Id);
            if (biddingSession == null)
                throw new KeyNotFoundException("Item không tồn tại!");
            if (biddingSession.Status == 1)
                throw new AppException("Phiên đấu thầu đang diễn ra, cập nhật thất bại!");
            if (biddingSession.Status == 2)
                throw new AppException("Phiên đấu thầu đã kết thúc, cập nhật thất bại!");
            if (itemModel.StartDate < Timestamp.UtcNow())
                throw new AppException("Thời gian diễn ra không được nhỏ hơn thời gian hiện tại!");
            if (itemModel.EndDate < Timestamp.UtcNow())
                throw new AppException("Thời gian kết thúc không được nhỏ hơn thời gian hiện tại!");
            if (itemModel.EndDate < itemModel.StartDate)
                throw new AppException("Thời gian kết thúc không được nhỏ hơn thời gian diễn ra!");
            if (itemModel.MaximumQuantity < itemModel.MinimumQuantity)
                throw new AppException("Số lượng tối đa không được nhỏ hơn số lương tối thiểu!");
            var bidding = await biddingService.GetByIdAsync(itemModel.BiddingId.Value);
            if (bidding == null)
                throw new AppException("Gói thầu không tồn tại");
            item.BiddingId = itemModel.BiddingId ?? biddingSession.BiddingId;
            await Task.Run(() =>
            {
                ObjectJobId objectJobIds = JsonConvert.DeserializeObject<ObjectJobId>(biddingSession.JobHangFireId);
                if (itemModel.StartDate != biddingSession.StartDate)
                {
                    if (!string.IsNullOrEmpty(objectJobIds.JobId1))
                    {
                        BackgroundJob.Delete(objectJobIds.JobId1);
                    }
                    // Job bắt sự kiện diễn ra của phiên thầu
                    var timeOutStartBidSession = (itemModel.StartDate - Timestamp.UtcNow());
                    TimeSpan secondTimeOutStart = TimeSpan.FromSeconds(timeOutStartBidSession.Value);
                    var jobBidSessionStartId = BackgroundJob.Schedule(() => biddingSessionService.ChangeStatusBiddingSessionStart(item), secondTimeOutStart);
                    // Lưu Jobid1 mới vào db
                    objectJobIds.JobId1 = jobBidSessionStartId;
                }

                if (itemModel.EndDate != biddingSession.EndDate)
                {
                    if (!string.IsNullOrEmpty(objectJobIds.JobId2))
                    {
                        BackgroundJob.Delete(objectJobIds.JobId2);
                    }
                    // Job bắt sự kiện kết thúc của phiên thầu
                    var timeOutEndBidSession = (itemModel.EndDate - Timestamp.UtcNow());
                    TimeSpan secondTimeOutEnd = TimeSpan.FromSeconds(timeOutEndBidSession.Value);
                    var jobBidSessionEndId = BackgroundJob.Schedule(() => biddingSessionService.ChangeStatusBiddingSessionEnd(item), secondTimeOutEnd);
                    // Lưu Jobid2 mới vào db
                    objectJobIds.JobId2 = jobBidSessionEndId;
                }
                item.JobHangFireId = JsonConvert.SerializeObject(objectJobIds);
            });
            bool success = await this.domainService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult() { ResultCode = (int)HttpStatusCode.OK, ResultMessage = "Cập nhật phiên thành công!", Success = true };
        }


        /// <summary>
        /// Danh sách phiên đấu thầu
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách phiên đấu thầu")]
        public override async Task<AppDomainResult> Get([FromQuery] BiddingSessionsSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_BiddingSessions> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<BiddingSessionModel> pagedDataModel = mapper.Map<PagedList<BiddingSessionModel>>(pagedData);

            if (baseSearch.ProviderId != null)
            {
                var ticketOfProvider = await biddingTicketService.GetAsync(x => x.CreatedBy == baseSearch.ProviderId);
                if (ticketOfProvider.Any() && ticketOfProvider != null)
                {
                    var bidSessionsId = ticketOfProvider.Where(x => x.BiddingSessionId != null).Select(y => y.BiddingSessionId);
                    if (bidSessionsId != null)
                    {
                        foreach (var item in bidSessionsId)
                        {
                            foreach (var jtem in pagedDataModel.Items)
                            {
                                if (jtem.Id == item)
                                {
                                    jtem.IsBid = true;
                                }
                                else
                                {
                                    jtem.IsBid = false;
                                }
                            }
                        }
                    }
                }
            }
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }
    }
}
