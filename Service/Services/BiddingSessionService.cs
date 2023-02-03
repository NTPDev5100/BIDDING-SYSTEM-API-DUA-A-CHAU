using AutoMapper;
using Entities;
using Entities.Search;
using Extensions;
using Interface.DbContext;
using Interface.Services;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;
using static Utilities.CoreContants;

namespace Service.Services
{
    public class BiddingSessionService : DomainServices.DomainService<tbl_BiddingSessions, BiddingSessionsSearch>, IBiddingSessionService
    {
        protected IAppDbContext coreDbContext;
        private IEmailConfigurationService emailConfigurationService;
        private IUserService userService;
        private INotificationService notificationService;
        private IBiddingTicketService biddingTicketService;
        private IBiddingService biddingService;
        private IProviderService providerService;

        public BiddingSessionService(IProviderService providerService, IBiddingService biddingService, IBiddingTicketService biddingTicketService, INotificationService notificationService, IUserService userService, IEmailConfigurationService emailConfigurationService, IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
            this.emailConfigurationService = emailConfigurationService;
            this.userService = userService;
            this.notificationService = notificationService;
            this.biddingTicketService = biddingTicketService;
            this.biddingService = biddingService;
            this.providerService = providerService;
        }

        protected override string GetStoreProcName()
        {
            return "BiddingSession_GetPagingData";
        }


        /// <summary>
        /// Hàm thay đổi trạng thái bắt đầu cho phiên thầu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ChangeStatusBiddingSessionStart(tbl_BiddingSessions item)
        {
            if (item != null)
            {
                //Thay đổi trạng thái phiên thầu => Status = (Đang diễn ra)
                item.Status = (int)StatusBiddingSession.DangDienRa;
                await this.UpdateFieldAsync(item, x => x.Status);


                //Tạo chủ đề và nội dung của thông báo
                string title = $"Thông báo: Phiên thầu {item.Name}!";
                string content = $"{item.Name} đã được bắt đầu.";

                //Các bước thông báo cho người dùng nhóm quản lý
                #region Các bước thông báo cho người dùng nhóm quản lý
                var exItemUser = new Expression<Func<tbl_Users, bool>>[]
                {
                               e => e.Roles.Contains(@"""RoleNumberLevel"":3") &&
                               e.Active == true
                };
                var managementUser = await userService.GetAsync(exItemUser);
                if (managementUser != null && managementUser.Any())
                {
                    var bidding = await biddingService.GetByIdAsync(item.BiddingId.Value);
                    string url = string.Empty;
                    if (bidding != null)
                    {
                        var urlJson = new ObjectJsonUrl
                        {
                            Id = bidding.Id,
                            Name = bidding.Name,
                            UrlReferrer = "/package/package-list/detail/"
                        };
                        url = JsonConvert.SerializeObject(urlJson);
                    }
                    // => 1:Tạo thông báo lưu xuống DB
                    await notificationService.CreateAsync(managementUser.Select(i => new tbl_Notification()
                    {
                        UserId = i.Id,
                        Title = title,
                        Content = content.ToString(),
                        IsSeen = false,
                        Created = Timestamp.UtcNow(),
                        Url = url,
                        IsType = (int)TypeNotification.PhienThauBatDau
                    }).ToList());

                    // => 2:Gửi thông báo qua Onesignal
                    List<string> oneSignal_DeviceIds = managementUser.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
                    string urlOfOneSignal = $"https://acp-achau.vercel.app/package/package-list/detail/?slug={bidding.Id}&Name={bidding.Name}";
                    OneSignalPush.OneSignalWebPushNotifications(title, content.ToString(), oneSignal_DeviceIds, urlOfOneSignal);

                    // => 3:Gửi thông báo qua mail
                    string[] tos = managementUser.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    if (tos.Any())
                    {
                        await emailConfigurationService.Send(title, content.ToString(), tos);
                    }
                }
                #endregion

                //Các bước Thông báo cho người dùng nhóm khách hàng
                #region Các bước Thông báo cho người dùng nhóm khách hàng
                var provider = await providerService.GetAsync(x => x.Active == true);
                if (provider != null || provider.Any())
                {
                    await notificationService.CreateAsync(provider.Select(i => new tbl_Notification()
                    {
                        UserId = i.Id,
                        Title = title,
                        Content = content.ToString(),
                        IsSeen = false,
                        Created = Timestamp.UtcNow(),
                        IsType = (int)TypeNotification.PhienThauBatDau
                    }).ToList());


                    // => 2:Gửi thông báo qua Onesignal
                    List<string> oneSignal_DeviceIds = provider.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
                    OneSignalPush.OneSignalMobilePushNotifications(title, content.ToString(), oneSignal_DeviceIds, string.Empty);
                }
                #endregion
            }
        }

        /// <summary>
        /// Hàm thay đổi trạng thái kết thúc cho phiên thầu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task ChangeStatusBiddingSessionEnd(tbl_BiddingSessions item)
        {
            if (item != null)
            {
                //Thay đổi trạng thái phiên thầu => Status = (Đã kết thúc)
                item.Status = (int)StatusBiddingSession.DaKetThuc;
                await this.UpdateFieldAsync(item, x => x.Status);
                //Tạo chủ đề và nội dung của thông báo
                string title = $"Thông báo: Phiên thầu {item.Name}!";
                string content = $"{item.Name} đã kết thúc.";

                //Các bước thông báo cho người dùng nhóm quản lý
                #region Các bước thông báo cho người dùng nhóm quản lý
                var exItemUser = new Expression<Func<tbl_Users, bool>>[]
                {
                               e => e.Roles.Contains(@"""RoleNumberLevel"":3") &&
                               e.Active == true
                };
                var managementUser = await userService.GetAsync(exItemUser);
                if (managementUser != null && managementUser.Any())
                {
                    var bidding = await biddingService.GetByIdAsync(item.BiddingId.Value);
                    string url = string.Empty;
                    if (bidding != null)
                    {
                        var urlJson = new ObjectJsonUrl
                        {
                            Id = bidding.Id,
                            Name = bidding.Name,
                            UrlReferrer = "/package/package-list/detail/"
                        };
                        url = JsonConvert.SerializeObject(urlJson);
                    }
                    // => 1:Tạo thông báo lưu xuống DB
                    await notificationService.CreateAsync(managementUser.Select(i => new tbl_Notification()
                    {
                        UserId = i.Id,
                        Title = title,
                        Content = content.ToString(),
                        IsSeen = false,
                        Created = Timestamp.UtcNow(),
                        Url = url,
                        IsType = (int)TypeNotification.PhienThauKetThuc
                    }).ToList());

                    // => 2:Gửi thông báo qua Onesignal
                    List<string> oneSignal_DeviceIds = managementUser.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
                    string urlOfOneSignal = $"https://acp-achau.vercel.app/package/package-list/detail/?slug={bidding.Id}&Name={bidding.Name}";
                    OneSignalPush.OneSignalWebPushNotifications(title, content.ToString(), oneSignal_DeviceIds, urlOfOneSignal);

                    // => 3:Gửi thông báo qua mail
                    string[] tos = managementUser.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    if (tos.Any())
                    {
                        await emailConfigurationService.Send(title, content.ToString(), tos);
                    }
                }
                #endregion

                //Các bước Thông báo cho người dùng nhóm khách hàng
                #region Các bước Thông báo cho người dùng nhóm khách hàng
                var provider = await providerService.GetAsync(x => x.Active == true);
                if (provider != null || provider.Any())
                {
                    // => 1:Tạo thông báo lưu xuống DB
                    await notificationService.CreateAsync(provider.Select(i => new tbl_Notification()
                    {
                        UserId = i.Id,
                        Title = title,
                        Content = content.ToString(),
                        IsSeen = false,
                        Created = Timestamp.UtcNow(),
                        IsType = (int)TypeNotification.PhienThauKetThuc
                    }).ToList());

                    // => 2:Gửi thông báo qua Onesignal
                    List<string> oneSignal_DeviceIds = provider.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
                    OneSignalPush.OneSignalMobilePushNotifications(title, content.ToString(), oneSignal_DeviceIds, string.Empty);
                }
                #endregion
            }
        }
        //Service thay đổi trạng thái của phiên đấu thầu tự động. 1 Phút sẽ chạy một lần để kiểm tra
        //public async Task BiddingSessionAuto()
        //{
        //    // Lấy các phiên có trạng thấy khác kết thúc
        //    var exItem = new Expression<Func<tbl_BiddingSessions, bool>>[]
        //    {
        //        e => e.Status != 2 && e.Deleted == false
        //    };
        //    var biddingSession = await this.GetAsync(exItem);
        //    if (biddingSession.Any())
        //    {
        //        DateTime date = DateTime.UtcNow;
        //        DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0);
        //        double result = Math.Round(date.Subtract(startTime).TotalSeconds, 0);
        //        foreach (var item in biddingSession)
        //        {
        //            //Nếu các phiên có ngày bắt đầu nhỏ hoặc bằng ngày hiện tại và ngày kết thúc lớn hơn ngày hiện tại
        //            if (item.StartDate <= result && item.EndDate > result && item.Status == 0)
        //            {
        //                // Đổi trạng thái = 1 bắt đầu
        //                item.Status = 1;

        //                string subject = "Thông báo: Phiên thầu bắt đầu!";
        //                StringBuilder content = new StringBuilder();
        //                content.Append($"{item.Name} đã được diễn ra.");
        //                //Thông báo cho quản lý
        //                var exItemUser = new Expression<Func<tbl_Users, bool>>[]
        //                {
        //                       e => e.Roles.Contains(@"""RoleNumberLevel"":3") &&
        //                       e.Deleted == false &&
        //                       e.Active == true
        //                };
        //                var managementUser = await userService.GetAsync(exItemUser);
        //                if (managementUser != null && managementUser.Any())
        //                {
        //                    var bidding = await biddingService.GetByIdAsync(item.BiddingId.Value);
        //                    string url = string.Empty;
        //                    if (bidding != null)
        //                    {
        //                        var urlJson = new ObjectJsonUrl
        //                        {
        //                            Id = bidding.Id,
        //                            Name = bidding.Name,
        //                            UrlReferrer = "/package/package-list/detail/"
        //                        };
        //                        url = JsonConvert.SerializeObject(urlJson);
        //                    }
        //                    //Tạo thông báo trong hệ thống
        //                    await notificationService.CreateAsync(managementUser.Select(i => new tbl_Notification()
        //                    {
        //                        UserId = i.Id,
        //                        Title = subject,
        //                        Content = content.ToString(),
        //                        IsSeen = false,
        //                        Created = Timestamp.UtcNow(),
        //                        Url = url
        //                    }).ToList());

        //                    //Gửi thông báo qua Onesignal
        //                    List<string> oneSignal_DeviceIds = managementUser.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
        //                    string urlOfOneSignal = $"https://acp-achau.vercel.app/package/package-list/detail/?slug={bidding.Id}&Name={bidding.Name}";
        //                    OneSignalPush.OneSignalWebPushNotifications(subject, content.ToString(), oneSignal_DeviceIds, urlOfOneSignal);

        //                    //Gửi mail
        //                    string[] tos = managementUser.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
        //                    if (tos.Any())
        //                    {
        //                        await emailConfigurationService.Send(subject, content.ToString(), tos);
        //                    }
        //                }

        //                //Thông báo cho khách hàng
        //                var exItemProvider = new Expression<Func<tbl_Providers, bool>>[]
        //                {
        //                    e => e.Deleted == false && e.Active == true
        //                };
        //                var provider = await providerService.GetAsync(exItemProvider);
        //                if (provider.Any())
        //                {
        //                    await notificationService.CreateAsync(provider.Select(i => new tbl_Notification()
        //                    {
        //                        UserId = i.Id,
        //                        Title = subject,
        //                        Content = content.ToString(),
        //                        IsSeen = false,
        //                        Created = Timestamp.UtcNow()
        //                    }).ToList());
        //                }
        //            }

        //            // Nếu các phiên có ngày kết thúc nhỏ hoặc bằng ngày hiện tại
        //            if (item.EndDate <= result && item.Status == 1)
        //            {
        //                //Đổi trạng thái = 2 kết thúc
        //                item.Status = 2;


        //                #region Thay đổi trạng thái cái phiếu thầu khi phiên hết thời gian
        //                //string titleResultTicket = $"kết quả đấu thầu {item.Name}";
        //                //string contentResultTicket = string.Empty;

        //                // Lấy các phiếu theo phiên kết thúc và có trạng thái chờ duyệt
        //                var exTicketNotApproval = new Expression<Func<tbl_BiddingTickets, bool>>[]
        //                {
        //                     e =>
        //                     e.Status == (int)StatucBiddingTicket.ChoDuyet  &&
        //                     e.Deleted == false &&
        //                     e.BiddingSessionId == item.Id &&
        //                     e.CreatedBy != null
        //                };
        //                //Thay đổi trảng thái chờ kết quả cho các phiếu chưa duyệt
        //                var ticketsNotApproval = await biddingTicketService.GetAsync(exTicketNotApproval);
        //                if (ticketsNotApproval.Any())
        //                {
        //                    foreach (var itemTicket in ticketsNotApproval)
        //                    {
        //                        itemTicket.Status = (int)StatucBiddingTicket.ChoKetQua;
        //                    }
        //                    await biddingTicketService.UpdateFieldAsync(ticketsNotApproval, x => x.Status);
        //                }
        //                // Lấy các phiếu theo phiên kết thúc và có kết quả trúng
        //                //var exTicketWin = new Expression<Func<tbl_BiddingTickets, bool>>[]
        //                //{
        //                //     e =>
        //                //     e.Status == 1  &&
        //                //     e.Deleted == false &&
        //                //     e.BiddingSessionId == item.Id &&
        //                //     e.CreatedBy != null
        //                //};
        //                //// Trả kết quả rớt thầu cho những phiếu chưa có kết quả
        //                //var ticketsWin = await biddingTicketService.GetAsync(exTicketWin);
        //                //if (ticketsWin.Any())
        //                //{
        //                //    contentResultTicket = $"Xin chúc mừng, bạn đã trúng phiên đấu thầu {item.Name}";
        //                //    await notificationService.CreateAsync(ticketsWin.Select(i => new tbl_Notification()
        //                //    {
        //                //        UserId = i.CreatedBy,
        //                //        Title = titleResultTicket,
        //                //        Content = contentResultTicket,
        //                //        IsSeen = false,
        //                //        Created = Timestamp.UtcNow()
        //                //    }).ToList());
        //                //    var providersIdInTicketWin = ticketsWin.Select(x => x.CreatedBy).ToList();
        //                //    var providers = await providerService.GetAsync(x => providersIdInTicketWin.Contains(x.Id) && x.Deleted == false && x.Active == true && !string.IsNullOrEmpty(x.Email));
        //                //    var emails = providers.Select(x => x.Email).ToArray();
        //                //    if (emails.Any())
        //                //    {
        //                //        string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
        //                //        Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
        //                //        var appRoot = appPathMatcher.Match(exePath).Value;
        //                //        var path = Path.Combine(appRoot, CoreContants.TEMPLATE_FOLDER_NAME, CoreContants.BIDDING_TICKET_RESULT_EMAIL_TEMPLATE_NAME);
        //                //        string body = string.Empty;
        //                //        using (StreamReader reader = new StreamReader(path))
        //                //        {
        //                //            body = reader.ReadToEnd();
        //                //        }
        //                //        body = body.Replace("{Name}", item.Name);
        //                //        body = body.Replace("{UrlThumbnailMail}", "http://acp.monamedia.net/86c7a49f-965b-4e94-9e8a-97c3546dde94-hinhnenmailxinchucmung.png");

        //                //        //Thông báo email
        //                //        await emailConfigurationService.Send(titleResultTicket.ToString(), body.ToString(), emails);
        //                //    }
        //                //}
        //                #endregion

        //                #region Thông báo phiên kết thúc cho nội bộ
        //                string subject = "Thông báo: phiên kết thúc!";
        //                StringBuilder content = new StringBuilder();
        //                content.Append($"{item.Name} đã kết thúc.");

        //                //Thông báo cho quản lý
        //                var exItemUser = new Expression<Func<tbl_Users, bool>>[]
        //                {
        //                     e => e.Roles.Contains(@"""RoleNumberLevel"":3")
        //                     && e.Deleted == false
        //                     && e.Active == true
        //                };
        //                var managementUser = await userService.GetAsync(exItemUser);
        //                if (managementUser != null && managementUser.Any())
        //                {
        //                    var bidding = await biddingService.GetByIdAsync(item.BiddingId.Value);
        //                    string url = string.Empty;
        //                    if (bidding != null)
        //                    {
        //                        var urlJson = new ObjectJsonUrl
        //                        {
        //                            Id = bidding.Id,
        //                            Name = bidding.Name,
        //                            UrlReferrer = "/package/package-list/detail/"
        //                        };
        //                        url = JsonConvert.SerializeObject(urlJson);
        //                    }
        //                    //Tạo thông báo trong hệ thống
        //                    await notificationService.CreateAsync(managementUser.Select(i => new tbl_Notification()
        //                    {
        //                        UserId = i.Id,
        //                        Title = subject,
        //                        Content = content.ToString(),
        //                        IsSeen = false,
        //                        Created = Timestamp.UtcNow(),
        //                        Url = url
        //                    }).ToList());

        //                    //Gửi thông báo qua Onesignal
        //                    List<string> oneSignal_DeviceIds = managementUser.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId)).Select(i => i.OneSignal_DeviceId).ToList();
        //                    string urlOfOneSignal = $"https://acp-achau.vercel.app/package/package-list/detail/?slug={bidding.Id}&Name={bidding.Name}";
        //                    OneSignalPush.OneSignalWebPushNotifications(subject, content.ToString(), oneSignal_DeviceIds, urlOfOneSignal);

        //                    //Gửi mail
        //                    string[] tos = managementUser.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
        //                    if (tos.Any())
        //                    {
        //                        await emailConfigurationService.Send(subject.ToString(), content.ToString(), tos);
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //        Expression<Func<tbl_BiddingSessions, object>>[] includeProperties = new Expression<Func<tbl_BiddingSessions, object>>[]
        //        {
        //            e => e.Status
        //        };
        //        await this.UpdateFieldAsync(biddingSession, includeProperties);
        //    }
        //}
    }
}
