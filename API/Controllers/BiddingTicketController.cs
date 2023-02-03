using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Interface.Services.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;
using static Utilities.CoreContants;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý phiếu đấu thầu
    /// </summary>
    [Route("api/biddingticket")]
    [ApiController]
    [Description("Quản lý phiếu đấu thầu")]
    [Authorize]
    public class BiddingTicketController : BaseController<tbl_BiddingTickets, BiddingTicketModel, BiddingTicketCreate, BiddingTicketUpdate, BiddingTicketsSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBiddingSessionService biddingSessionService;
        private readonly IBiddingService biddingService;
        private readonly IProviderService providerService;
        private readonly IConfiguration configuration;
        private readonly INotificationService notificationService;
        private readonly IUserService userService;
        private readonly IBiddingTicketService biddingTicketService;
        static  object locker = new object();
        public BiddingTicketController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_BiddingTickets, BiddingTicketModel, BiddingTicketCreate, BiddingTicketUpdate, BiddingTicketsSearch>> logger,
            IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(serviceProvider, logger, env)
        {
            this.configuration = configuration;
            biddingService = serviceProvider.GetRequiredService<IBiddingService>();
            providerService = serviceProvider.GetRequiredService<IProviderService>();
            biddingSessionService = serviceProvider.GetRequiredService<IBiddingSessionService>();
            notificationService = serviceProvider.GetRequiredService<INotificationService>();
            userService = serviceProvider.GetRequiredService<IUserService>();
            domainService = serviceProvider.GetRequiredService<IBiddingTicketService>();
            biddingTicketService = serviceProvider.GetRequiredService<IBiddingTicketService>();
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Thêm mới phiếu đấu thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới phiếu đấu thầu")]
        public override async Task<AppDomainResult> AddItem([FromBody] BiddingTicketCreate itemModel)
        {
            lock (locker)
            {
                if (!ModelState.IsValid)
                    throw new AppException(ModelState.GetErrorMessage());
                var item = mapper.Map<tbl_BiddingTickets>(itemModel);
                if (item == null)
                    throw new AppException("Item không tồn tại!");
                // Kiểm tra phiếu bỏ thầu của nhà cung cấp có tồn tại ko
                var userLogin = LoginContext.Instance.CurrentUser;
                // Kiểm tra nhà cung cấp đã bỏ thầu cho phiên này chưa 
                var checkExistTicket = biddingTicketService.CheckTicketExist(userLogin.userId, itemModel.BiddingSessionId.Value);
                if (!string.IsNullOrEmpty(checkExistTicket))
                    throw new Exception(checkExistTicket.ToString());
                DateTime date = DateTime.UtcNow;
                DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
                double now = Math.Round(date.Subtract(startTime).TotalSeconds, 0);
                var biddingSession =  biddingSessionService.GetById(itemModel.BiddingSessionId.Value);
                if (biddingSession == null)
                    throw new AppException("Phiên thầu không tồn tại!");
                if (biddingSession.StartDate > now)
                    throw new AppException("Phiên đấu thầu chưa được diễn ra, xin vui lòng chờ thêm!");
                if (biddingSession.EndDate < now)
                    throw new AppException("Phiên đấu thầu đã kết thúc, xin vui lòng chọn phiên khác!");
                if (biddingSession.MinimumQuantity > itemModel.Quantity || biddingSession.MaximumQuantity < item.Quantity)
                    throw new AppException("Số lượng không hợp lệ, vui lòng nhập lại!");
                item.BiddingSessionId = biddingSession.Id;
                item.BiddingId = biddingSession.BiddingId;
                item.Status = (int)StatucBiddingTicket.ChoDuyet;
                bool success = this.domainService.Save(item);
                if (!success)
                    throw new AppException("Lỗi trong quá trình xử lý");              
            }
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới phiếu thành công!",
                Success = true
            };
        }
        /// <summary>
        /// Cập nhật kết quả phiếu đấu thầu
        /// </summary>
        /// <param name="itemModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update-result-bidding-ticket/{id}")]
        [AppAuthorize]
        [Description("Cập nhật kết quả phiếu thầu")]
        public virtual async Task<AppDomainResult> UpdateResultBiddingTicket([FromBody] BidTicketStatusModel itemModel, Guid? id)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var ticket = await this.domainService.GetByIdAsync(id.Value);
            if (ticket == null)
                throw new AppException("Phiếu thầu không tồn tại!");
            var biddingSession = await this.biddingSessionService.GetByIdAsync(ticket.BiddingSessionId.Value);
            string content = string.Empty;
            string title = string.Empty;
            int isType = 0;
            switch (itemModel.Status.Value)
            {
                // Trúng thầu (2)  Rớt thầu (3)
                case (int)StatucBiddingTicket.TrungThau:
                    ticket.Status = 2;
                    title = $"Đã có kết quả đấu thầu: {biddingSession.Name ?? string.Empty}";
                    content = $"Bạn đã trúng thầu: {biddingSession.Name ?? string.Empty}";
                    isType = (int)TypeNotification.TrungThau;
                    break;
                case (int)StatucBiddingTicket.RotThau:
                    ticket.Status = 3;
                    title = $"Đã có kết quả đấu thầu: {biddingSession.Name ?? string.Empty}";
                    content = $"Bạn đã rớt thầu: {biddingSession.Name ?? string.Empty}";
                    isType = (int)TypeNotification.RotThau;
                    break;
            }
            ticket.UpdatedBy = LoginContext.Instance.CurrentUser.userId;
            ticket.Updated = Timestamp.UtcNow();
            Expression<Func<tbl_BiddingTickets, object>>[] includeProperties = new Expression<Func<tbl_BiddingTickets, object>>[]
            {
                e => e.Status,
                e => e.UpdatedBy,
                e => e.Updated
            };
            bool success = await this.domainService.UpdateFieldAsync(ticket, includeProperties);
            if (!success)
                throw new Exception("Duyệt kết quả thất bại!");

            //Thông báo kết quả phiếu thầu cho ncc
            await notificationService.CreateAsync(new tbl_Notification()
            {
                UserId = ticket.CreatedBy.Value,
                Title = title,
                Content = content,
                IsSeen = false,
                Created = Timestamp.UtcNow(),
                IsType = isType
            });
            // => 2:Gửi thông báo qua Onesignal
            var provider = await providerService.GetByIdAsync(ticket.CreatedBy.Value);
            if (provider != null && !string.IsNullOrEmpty(provider.OneSignal_DeviceId))
            {
                List<string> oneSignal_DeviceIds = new List<string> { provider.OneSignal_DeviceId };
                OneSignalPush.OneSignalMobilePushNotifications(title, content.ToString(), oneSignal_DeviceIds, string.Empty);
            }

            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Duyệt kết quả thành công!",
                Success = true
            };


        }


        /// <summary>
        /// Danh sách phiếu đấu thầu
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách phiếu đấu thầu")]
        public override async Task<AppDomainResult> Get([FromQuery] BiddingTicketsSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var userLogin = LoginContext.Instance.CurrentUser;
            bool checkRoleLogin = false;
            if (!string.IsNullOrEmpty(userLogin.roles))
                checkRoleLogin = userLogin.roles.Contains(@"""RoleNumberLevel"":2");

            if (!userLogin.isAdmin && checkRoleLogin)
            {
                baseSearch.role = "staff";
                //var onGoingSession = await this.biddingSessionService.GetAsync(x => x.Status != (int)StatusBiddingSession.DangDienRa);
                //if (onGoingSession.Any())
                //    baseSearch.BiddingSessionId = String.Join(',', onGoingSession.Select(x => x.Id));
            }
            PagedList<tbl_BiddingTickets> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<BiddingTicketModel> pagedDataModel = mapper.Map<PagedList<BiddingTicketModel>>(pagedData);
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };

        }


        /// <summary>
        /// Duyệt kết quả các phiếu thầu được chọn
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut("approval-off-selected-bids-ticket")]
        [AppAuthorize]
        [Description("Duyệt kết quả các phiếu thầu được chọn")]
        public async Task<AppDomainResult> ApprovalOfSelectedBids([FromBody] BidTicketStatusListModel itemModel)
        {
            bool success = true;
            if (itemModel.ListId == null || !itemModel.ListId.Any())
                throw new AppException("Không có thông tin phiếu bỏ thầu!");

            var biddingTickets = await this.domainService.GetAsync(x => !x.Deleted.Value &&
            (itemModel.ListId == null || !itemModel.ListId.Any() || itemModel.ListId.Contains(x.Id)));
            if (biddingTickets != null || biddingTickets.Any())
            {
                List<int> listStatus = new List<int>() { 0, 1 };
                var biddingTicketsUnapproved = biddingTickets.Where(x => listStatus.Contains(x.Status.Value)).ToList();
                switch (itemModel.Status.Value)
                {
                    // Trúng thầu (2)  Rớt thầu (3)
                    case (int)StatucBiddingTicket.TrungThau:
                        biddingTicketsUnapproved.ForEach(x => { x.Status = 2; x.Updated = Timestamp.UtcNow(); x.UpdatedBy = LoginContext.Instance.CurrentUser.userId; });
                        break;
                    case (int)StatucBiddingTicket.RotThau:
                        biddingTicketsUnapproved.ForEach(x => { x.Status = 3; x.Updated = Timestamp.UtcNow(); x.UpdatedBy = LoginContext.Instance.CurrentUser.userId; });
                        break;
                }
                Expression<Func<tbl_BiddingTickets, object>>[] includeProperties = new Expression<Func<tbl_BiddingTickets, object>>[]
                {
                            e => e.Status,
                            e => e.UpdatedBy,
                            e => e.Updated
                };
                //Gửi thông báo
                string content = string.Empty;
                string title = string.Empty;
                int isType = 0;
                foreach (var ticket in biddingTicketsUnapproved)
                {
                    var biddingSession = await this.biddingSessionService.GetByIdAsync(ticket.BiddingSessionId.Value);
                    if (itemModel.Status.Value == 2)
                    {
                        title = $"Đã có kết quả đấu thầu: {biddingSession.Name ?? string.Empty}";
                        content = $"Bạn đã trúng thầu: {biddingSession.Name ?? string.Empty}";
                        isType = (int)TypeNotification.TrungThau;
                    }
                    if (itemModel.Status.Value == 3)
                    {
                        title = $"Đã có kết quả đấu thầu: {biddingSession.Name ?? string.Empty}";
                        content = $"Bạn đã rớt thầu: {biddingSession.Name ?? string.Empty}";
                        isType = (int)TypeNotification.RotThau;
                    }
                    await notificationService.Send(ticket.CreatedBy, LoginContext.Instance.CurrentUser.userId, title, content, isType);
                    // => 2:Gửi thông báo qua Onesignal
                    var provider = await providerService.GetByIdAsync(ticket.CreatedBy.Value);
                    if (provider != null && !string.IsNullOrEmpty(provider.OneSignal_DeviceId))
                    {
                        List<string> oneSignal_DeviceIds = new List<string> { provider.OneSignal_DeviceId };
                        OneSignalPush.OneSignalMobilePushNotifications(title, content.ToString(), oneSignal_DeviceIds, string.Empty);
                    }
                }
                success &= await this.domainService.UpdateFieldAsync(biddingTicketsUnapproved, includeProperties);

            }
            else throw new AppException("Không có phiếu nào trong danh sách!");
            return new AppDomainResult()
            {
                Success = success,
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Duyệt kết quả thành công!"
            };
        }


        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        //[HttpGet("export-excel")]
        //[AppAuthorize]
        //[Description("Xuất dữ liệu excel")]
        [NonAction]
        public async Task<AppDomainResult> ExportExcel([FromQuery] BiddingTicketsSearch baseSearch)
        {
            PagedList<tbl_BiddingTickets> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<BiddingTicketModel> pagedDataModel = mapper.Map<PagedList<BiddingTicketModel>>(pagedData);
            //Tạo table template cần xuất excel
            DataTable dt = new DataTable();
            await Task.Run(() =>
            {
                //Setting Table Name
                dt.TableName = "Danh sách phiếu bỏ thầu";
                //Add columns
                dt.Columns.Add("Công ty", typeof(string));
                dt.Columns.Add("Họ tên", typeof(string));
                dt.Columns.Add("Tên phiên thầu", typeof(string));
                dt.Columns.Add("Số lượng", typeof(int));
                dt.Columns.Add("Giá", typeof(double));
                dt.Columns.Add("Ngày bỏ thầu", typeof(string));
                dt.Columns.Add("Kết quả", typeof(string));
                //Nếu danh sách từ bộ lọc tồn tại item
                if (pagedDataModel.Items.Any())
                {
                    foreach (var item in pagedDataModel.Items)
                    {
                        //Add rows
                        try
                        {
                            DataRow dr = dt.Rows.Add();
                            dr["Công ty"] = item.CompanyName;
                            dr["Họ tên"] = item.FullName;
                            dr["Tên phiên thầu"] = item.BiddingSessionName;
                            dr["Số lượng"] = item.QuantityFormat;
                            dr["Giá"] = item.PriceFormat;
                            dr["Ngày bỏ thầu"] = item.DateCreate;
                            dr["Kết quả"] = item.StatusName;
                            dt.Rows.Add(dr);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            });
            //Tạo content muốn có trong excel
            var objectExcel = new ObjectExcel
            {
                titleTable = "DANH SÁCH PHIẾU BỎ THẦU",
                headerCell = "CÔNG TY CỔ PHẦN CHẾ BIẾN DỪA Á CHÂU"
            };
            //Lấy ra file excel đã được build
            ExcelUtilities excelUtility = new ExcelUtilities();
            var workbook = excelUtility.BuildFileExcel(dt, objectExcel);
            var appDomainResult = new AppDomainResult
            {
                Success = true,
                Data = workbook,
                ResultCode = (int)HttpStatusCode.OK
            };
            return appDomainResult;
        }


        #region Excel
        /// <summary>
        /// Xuất Excel
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <param name="typeListTicket"></param>
        /// <returns></returns>
        [HttpGet("export-excel-bidding-ticket")]
        public virtual async Task<AppDomainResult> ExportExcel([FromQuery] BiddingTicketsSearch baseSearch, int typeListTicket)
        {
            string fileResultPath = string.Empty;
            var userLogin = LoginContext.Instance.CurrentUser;
            bool checkRoleLogin = false;
            if (!string.IsNullOrEmpty(userLogin.roles))
                checkRoleLogin = userLogin.roles.Contains(@"""RoleNumberLevel"":2");

            if (!userLogin.isAdmin && checkRoleLogin)
            {
                baseSearch.role = "staff";                
            }
            PagedList<BiddingTicketModel> pagedListModel = new PagedList<BiddingTicketModel>();
            //............................ Lấy thông tin xuất excel
            // 1. Lấy thông tin data và đổ data vào template
            PagedList<tbl_BiddingTickets> pagedData = await this.domainService.GetPagedListData(baseSearch);
            pagedListModel = mapper.Map<PagedList<BiddingTicketModel>>(pagedData);
            if (!pagedListModel.Items.Any() || pagedListModel.Items == null)
                throw new AppException("Danh sách trống, xuất excel thất bại!");
            ExcelUtilities excelUtility = new ExcelUtilities();
            // 2. Lấy thông tin file template để export
            string fileName = string.Empty;
            string getTemplateFilePath = string.Empty;
            var roleNumbers = await userService.GetRoleNumberLevel(LoginContext.Instance.CurrentUser.userId);
            if (roleNumbers.Contains((int)RoleNumberLevel.QuanLy) || LoginContext.Instance.CurrentUser.isAdmin)
            {
                if (typeListTicket == (int)TypeFileExcelBidTicket.DanhSachLichSuTheoPhien)
                {
                    fileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"), "DanhSachLichSuPhieuThau");
                    getTemplateFilePath = GetTemplateFilePath("BiddingTicketHistoryTemplate.xlsx");
                }
                if (typeListTicket == (int)TypeFileExcelBidTicket.DanhSachTong)
                {
                    fileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"), "DanhSachPhieuThau");
                    getTemplateFilePath = GetTemplateFilePath("BiddingTicketTemplate.xlsx");
                }
            }
            if (roleNumbers.Contains((int)RoleNumberLevel.NhanVien))
            {
                if (typeListTicket == (int)TypeFileExcelBidTicket.DanhSachLichSuTheoPhien)
                {
                    fileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"), "DanhSachLichSuPhieuThau");
                    getTemplateFilePath = GetTemplateFilePath("BiddingTicketHistoryNotResultTemplate.xlsx");
                }
                if (typeListTicket == (int)TypeFileExcelBidTicket.DanhSachTong)
                {
                    fileName = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"), "DanhSachPhieuThau");
                    getTemplateFilePath = GetTemplateFilePath("BiddingTicketNotResultTemplate.xlsx");
                }
            }
            excelUtility.TemplateFileData = System.IO.File.ReadAllBytes(getTemplateFilePath);
            // 3. LẤY THÔNG TIN THAM SỐ TRUYỀN VÀO
            excelUtility.ParameterData = await GetParameterReport(pagedListModel, baseSearch);
            if (pagedListModel.Items == null || !pagedListModel.Items.Any())
                pagedListModel.Items.Add(new BiddingTicketModel());
            byte[] fileByteReport = excelUtility.Export(pagedListModel.Items);

            // Xuất biểu đồ nếu có
            fileByteReport = await this.ExportChart(fileByteReport, pagedListModel.Items);

            // 4. LƯU THÔNG TIN FILE BÁO CÁO XUỐNG FOLDER BÁO CÁO

            string filePath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME, fileName);

            string folderUploadPath = string.Empty;
            //var folderUpload = configuration.GetValue<string>("MySettings:FolderUpload");
            folderUploadPath = Path.Combine(env.ContentRootPath, CoreContants.UPLOAD_FOLDER_NAME);
            string fileUploadPath = Path.Combine(folderUploadPath, Path.GetFileName(filePath));

            FileUtilities.CreateDirectory(folderUploadPath);
            FileUtilities.SaveToPath(fileUploadPath, fileByteReport);

            var currentLinkSite = string.Empty;
            if (Extensions.HttpContext.Current.Request.IsHttps)
                currentLinkSite = $"{Extensions.HttpContext.Current.Request.Scheme}://{Extensions.HttpContext.Current.Request.Host}";
            else
                currentLinkSite = $"{Extensions.HttpContext.Current.Request.Scheme}s://{Extensions.HttpContext.Current.Request.Host}";
            fileResultPath = Path.Combine(currentLinkSite, Path.GetFileName(filePath));

            // 5. TRẢ ĐƯỜNG DẪN FILE CHO CLIENT DOWN VỀ
            return new AppDomainResult()
            {
                Data = fileResultPath,
                ResultCode = (int)HttpStatusCode.OK,
                Success = true,
            };


        }

        protected async Task<byte[]> ExportChart(byte[] excelData, IList<BiddingTicketModel> listData)
        {
            return excelData;
        }

        /// <summary>
        /// Lấy đường dẫn file template
        /// </summary>
        /// <param name="fileTemplateName"></param>
        /// <returns></returns>
        protected virtual string GetTemplateFilePath(string fileTemplateName)
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, CoreContants.TEMPLATE_FOLDER_NAME, fileTemplateName);
            if (!System.IO.File.Exists(path))
                throw new AppException("File template không tồn tại!");
            return path;
        }

        /// <summary>
        /// Lấy thông số parameter truyền vào
        /// </summary>
        /// <param name="pagedList"></param>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        protected virtual async Task<IDictionary<string, object>> GetParameterReport(PagedList<BiddingTicketModel> pagedList, BiddingTicketsSearch baseSearch)
        {
            return await Task.Run(() =>
            {
                IDictionary<string, object> dictionaries = new Dictionary<string, object>();
                return dictionaries;
            });
        }
        #endregion
    }
}
