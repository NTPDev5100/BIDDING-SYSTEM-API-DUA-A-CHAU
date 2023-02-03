using AutoMapper;
using Entities;
using Entities.Search;
using Interface.DbContext;
using Interface.Services;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Newtonsoft.Json;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace Service.Services
{
    public class NotificationService : DomainService<tbl_Notification, NotificationsSearch>, INotificationService
    {
        protected IAppDbContext coreDbContext;
        private IUserService userService;
        private IEmailConfigurationService emailConfigurationService;

        public NotificationService(IEmailConfigurationService emailConfigurationService, IUserService userService, IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
            this.userService = userService;
            this.emailConfigurationService = emailConfigurationService;
        }
        protected override string GetStoreProcName()
        {
            return "Notification_GetPagingData";
        }

        public async Task Send(Guid? userId, Guid? createdById, string title, string content, int? isType)
        {
            var notificationModel = new tbl_Notification()
            {
                Title = title,
                Content = content,
                UserId = userId,
                IsSeen = false,
                CreatedBy = createdById,
                Active = true,
                Created = Timestamp.UtcNow(),
                IsType = isType
            };
            await CreateAsync(notificationModel);                                 
        }



        public async Task OneSignalPushNotifications(string headings, string content, string OneSignal_PlayerId)
        {
            try
            {
                if (OneSignal_PlayerId.Any())
                {
                    string onesignalAppId = "72ba8155-836a-4013-abaa-a787681eb5eb";//cái này sửa lại
                    string onesignalRestId = "YTJjM2ZjZmUtN2ExNi00NDhlLTk4YWEtMjQ1MGZiZDMyMTFl";//cái này sửa lại

                    var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Headers.Add("authorization", "Basic " + onesignalRestId);

                    var obj = new
                    {
                        app_id = onesignalAppId,
                        headings = new { en = headings },
                        contents = new { en = content },
                        channel_for_external_user_ids = "push",
                        //include_player_ids = new string[] { "4ecd269c-7356-11ec-9a39-2255d3251ce2" }//Gửi cho user đc chỉ định
                        include_player_ids = new string[] { OneSignal_PlayerId },//Gửi cho user đc chỉ định
                        //included_segments = new string[] { "Subscribed Users" } //Gửi cho tất cả user nào đăng ký
                    };
                    var param = JsonConvert.SerializeObject(obj);
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);

                    string responseContent = null;

                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }
                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
            }
            catch { }
        }
    }
}
