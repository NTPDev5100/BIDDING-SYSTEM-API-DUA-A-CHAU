using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý thông báo
    /// </summary>
    [Route("api/notification")]
    [ApiController]
    [Description("Quản lý thông báo")]
    [Authorize]
    public class NotificationController : BaseController<tbl_Notification, NotificationModel, NotificationCreate, NotificationUpdate, NotificationsSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_Notification, NotificationModel, NotificationCreate, NotificationUpdate, NotificationsSearch>> logger
            , IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<INotificationService>();
            this._httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// Danh sách thông báo
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách thông báo")]
        public override async Task<AppDomainResult> Get([FromQuery] NotificationsSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_Notification> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<NotificationModel> pagedDataModel = mapper.Map<PagedList<NotificationModel>>(pagedData);
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Xem thông báo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("seen/{id}")]
        [Authorize]
        [Description("Xem thông báo")]
        public virtual async Task<AppDomainResult> Seen(Guid id)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var noti = await this.domainService.GetByIdAsync(id);
            if (noti == null)
                throw new AppException("Thông báo không tồn tại");
            noti.IsSeen = true;
            noti.UpdatedBy = LoginContext.Instance.CurrentUser.userId;
            noti.Updated = Timestamp.UtcNow();
            Expression<Func<tbl_Notification, object>>[] includeProperties = new Expression<Func<tbl_Notification, object>>[]
            {
                e => e.IsSeen,
                e => e.UpdatedBy,
                e => e.Updated
            };
            bool success = await this.domainService.UpdateFieldAsync(noti, includeProperties);
            if (!success)
                throw new Exception("Xem thông báo thất bại!");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Xem thông báo thành công!",
                Success = true
            };
        }

        /// <summary>
        /// Xem tất cả thông báo
        /// </summary>
        /// <returns></returns>
        [HttpPut("seen-all")]
        [Authorize]
        [Description("Xem tất cả thông báo")]
        public virtual async Task<AppDomainResult> SeenAll()
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var userLogin = LoginContext.Instance.CurrentUser;
            var exItem = new Expression<Func<tbl_Notification, bool>>[]
            {
                                    e => e.UserId == userLogin.userId && e.Active == true && e.IsSeen == false
            };
            var notifications = await domainService.GetAsync(exItem);
            if (notifications.Any())
            {
                foreach (var item in notifications)
                {
                    var notification = await domainService.GetByIdAsync(item.Id);
                    notification.IsSeen = true;
                    notification.UpdatedBy = userLogin.userId;
                    notification.Updated = Timestamp.UtcNow();
                    Expression<Func<tbl_Notification, object>>[] includeProperties = new Expression<Func<tbl_Notification, object>>[]
                    {
                        e => e.IsSeen,
                        e => e.UpdatedBy,
                        e => e.Updated
                    };
                    bool success = await this.domainService.UpdateFieldAsync(notification, includeProperties);
                }
            }
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Xem thông báo thành công!",
                Success = true
            };
        }



    }
}
