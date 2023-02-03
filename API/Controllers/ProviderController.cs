using BaseAPI.Controllers;
using Entities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Interface.Services.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Utilities;
using static Utilities.CatalogueEnums;
using static Utilities.CoreContants;

namespace API.Controllers
{
    /// <summary>
    /// Quản lý nhà cung cấp
    /// </summary>
    [Route("api/provider")]
    [ApiController]
    [Description("Quản lý nhà cung cấp")]
    [Authorize]
    public class ProviderController : BaseController<tbl_Providers, ProviderModel, ProviderCreate, ProviderUpdate, ProviderSearch>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly IEmailConfigurationService emailConfigurationService;
        private readonly INotificationService notificationService;
        private readonly IProviderService providerService;
        public ProviderController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_Providers, ProviderModel, ProviderCreate, ProviderUpdate, ProviderSearch>> logger
            , IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            providerService = serviceProvider.GetRequiredService<IProviderService>();
            domainService = serviceProvider.GetRequiredService<IProviderService>();
            userService = serviceProvider.GetRequiredService<IUserService>();
            roleService = serviceProvider.GetRequiredService<IRoleService>();
            _httpContextAccessor = httpContextAccessor;
            emailConfigurationService = serviceProvider.GetRequiredService<IEmailConfigurationService>();
            notificationService = serviceProvider.GetRequiredService<INotificationService>();
        }



        /// <summary>
        /// Thêm mới nhà cung cấp
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới nhà cung cấp")]
        public override async Task<AppDomainResult> AddItem([FromBody] ProviderCreate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Providers>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại");

            //Kiểm tra item có tồn tại không (UserName,Email,Phone)
            var messageProviderCheck = await domainService.GetExistItemMessage(item);
            if (!string.IsNullOrEmpty(messageProviderCheck))
                throw new AppException(messageProviderCheck);
            if (!string.IsNullOrEmpty(itemModel.Email))
            {
                bool mail = new EmailAddressAttribute().IsValid(itemModel.Email);
                if (!mail)
                    throw new AppException("Mail không đúng định dạng!");
            }
            item.Password = SecurityUtilities.HashSHA1(item.Password);
            item.Username = (item.Username).Replace(" ", "").Trim();
            //Chọn nhân viên phụ trách. Có thể có 1 hoặc nhiều nhân viên phụ trách
            string personInCharge = await providerService.GetPersonCharge(itemModel.PersonInCharge);
            if (string.IsNullOrEmpty(personInCharge))
                throw new AppException("Nhân viên phụ trách không tồn tại!");
            item.PersonInCharge = personInCharge;
            //Add role Khách hàng cho nhà cung cấp
            List<ObjectJsonRole> roleInUsers = new List<ObjectJsonRole>();
            var role = await roleService.GetSingleAsync(x => x.RoleNumberLevel == (int)RoleNumberLevel.KhachHang && x.Deleted == false);
            if (role == null)
                throw new AppException("Quyền không tồn tại");
            roleInUsers.Add(new ObjectJsonRole { Id = role.Id, Name = role.Name, RoleNumberLevel = role.RoleNumberLevel ?? (int)RoleNumberLevel.KhachHang });
            item.Roles = JsonConvert.SerializeObject(roleInUsers);
            //Logic xử lý trạng thái của ncc
            //Logic gửi thông báo web và email
            var userLogin = LoginContext.Instance.CurrentUser;
            bool checkRoleLogin = false;
            if (!string.IsNullOrEmpty(userLogin.roles))
                checkRoleLogin = userLogin.roles.Contains(@"""RoleNumberLevel"":2");
            if (!userLogin.isAdmin && checkRoleLogin)
            {
                item.Active = false;
                //Lấy những user có role quản lý
                var managementUser = await userService.GetAsync(e => e.Roles.Contains(@"""RoleNumberLevel"":3") && e.Active == true);
                if (managementUser.Any())
                {
                    //Bước gửi thông báo nhà cung cấp chờ duyệt qua email cho quản lý                        
                    //Tiêu đề thông báo
                    string title = "Nhà cung cấp mới";
                    //Nội dung thông báo
                    StringBuilder contentNotification = new StringBuilder();
                    contentNotification.AppendLine($"Nhà cung cấp {itemModel.FullName} đang được chờ duyệt.");
                    contentNotification.AppendLine("Nhấp vào thông báo để xem chi tiết nhà cung cấp.");

                    var urlJson = new ObjectJsonUrl
                    {
                        Id = Guid.Empty,
                        Name = string.Empty,
                        UrlReferrer = "/users/supplier/new-suppliers"
                    };
                    string url = JsonConvert.SerializeObject(urlJson);
                    await notificationService.CreateAsync(managementUser.Select(i => new tbl_Notification()
                    {
                        UserId = i.Id,
                        Title = title,
                        Content = contentNotification.ToString(),
                        IsSeen = false,
                        Created = Timestamp.UtcNow(),
                        Url = url
                    }).ToList());

                    var OneSignal_DeviceIds = managementUser.Where(x => !string.IsNullOrEmpty(x.OneSignal_DeviceId) && x.OneSignal_DeviceId != null).Select(i => i.OneSignal_DeviceId).ToList();
                    var urlOfOneSignal = _httpContextAccessor.HttpContext.Request.Headers["Origin"] + "/users/supplier/new-suppliers";
                    OneSignalPush.OneSignalWebPushNotifications(title, contentNotification.ToString(), OneSignal_DeviceIds, urlOfOneSignal);

                    string[] tos = managementUser.Where(x => !string.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToArray();
                    if (tos.Any())
                    {
                        string body = string.Empty;
                        var path = Path.Combine(env.ContentRootPath, CoreContants.TEMPLATE_FOLDER_NAME, CoreContants.CREATE_NEW_PROVIDER_EMAIL_TEMPLATE_NAME);
                        using (StreamReader reader = new StreamReader(path))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{UserName}", item.FullName);
                        //body = body.Replace("{UrlThumbnailMail}", "https://acp.monamedia.net/473e4f12-71b2-4a7b-8efb-2d7a953a1951-hinhnenmail.png");
                        //Thông báo email
                        await emailConfigurationService.Send(title, body.ToString(), tos);
                    }
                }
            }
            bool success = await this.domainService.CreateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý!");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới nhà cung cấp thành công",
                Success = true
            };
        }


        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật nhà cung cấp")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] ProviderUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            var item = mapper.Map<tbl_Providers>(itemModel);
            if (item == null)
                throw new KeyNotFoundException("Item không tồn tại");
            //Kiểm tra item có tồn tại không (UserName,Email,Phone)
            var messageProviderCheck = await domainService.GetExistItemMessage(item);
            if (!string.IsNullOrEmpty(messageProviderCheck))
                throw new AppException(messageProviderCheck);
            if (!string.IsNullOrEmpty(itemModel.Password))
            {
                if (itemModel.Password.Length < 8)
                    throw new AppException("Mật khẩu phải lớn hơn 8 kí tự");
                item.Password = SecurityUtilities.HashSHA1(item.Password);
            }
            var providerOld = await domainService.GetByIdAsync(item.Id);
            if (string.IsNullOrEmpty(itemModel.Password))
            {
                item.Password = providerOld.Password;
            }
            if (!string.IsNullOrEmpty(itemModel.Email))
            {
                bool mail = new EmailAddressAttribute().IsValid(itemModel.Email);
                if (!mail)
                    throw new AppException("Mail không đúng định dạng!");
            }
            var userLoginCurrent = LoginContext.Instance.CurrentUser;
            bool loginCheckCustomer = false;
            if (!userLoginCurrent.isAdmin)
                loginCheckCustomer = userLoginCurrent.roles.Contains(@"""RoleNumberLevel"":1");
            //Nếu là nhân viên nội bộ update =>
            if (!loginCheckCustomer || userLoginCurrent.isAdmin)
            {
                if (string.IsNullOrEmpty(itemModel.Username))
                    throw new AppException("Vui lòng nhập tên đăng nhập!");
                item.Username = (item.Username).Replace(" ", "").Trim();
                if (!string.IsNullOrEmpty(itemModel.PersonInCharge))
                {
                    //Chọn nhân viên phụ trách. Có thể có 1 hoặc nhiều nhân viên phụ trách
                    List<ObjectJsonCustom> personInChargeOfProvider = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(itemModel.PersonInCharge);
                    List<ObjectJsonCustom> personInChargeInProvider = new List<ObjectJsonCustom>();
                    foreach (ObjectJsonCustom itemPersonInCharge in personInChargeOfProvider)
                    {
                        var person = await userService.GetByIdAsync(itemPersonInCharge.Id);
                        if (person == null)
                            throw new AppException("Nhân viên phụ trách không tồn tại");
                        personInChargeInProvider.Add(new ObjectJsonCustom { Id = person.Id, Name = person.FullName });
                    }
                    item.PersonInCharge = JsonConvert.SerializeObject(personInChargeInProvider);
                }
                //Nếu không chọn nhân viên phụ trách, thì lấy nhân viên đang login
                if (string.IsNullOrEmpty(itemModel.PersonInCharge))
                {
                    List<ObjectJsonCustom> personInChargeInProvider = new List<ObjectJsonCustom>();
                    personInChargeInProvider.Add(new ObjectJsonCustom { Id = LoginContext.Instance.CurrentUser.userId, Name = LoginContext.Instance.CurrentUser.fullName });
                    item.PersonInCharge = JsonConvert.SerializeObject(personInChargeInProvider);
                }
                //Kiểm tra quản lý có kích hoạt cho tài khoản ncc ko. Nếu có sẽ thông báo qua email cho ncc
                if (!string.IsNullOrEmpty(item.Email))
                {
                    if (providerOld.Active == false && itemModel.Active == true)
                    {
                        List<ObjectJsonCustom> personInChargeOfProvider = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(item.PersonInCharge);
                        string[] tos = new string[] { item.Email };
                        string body = string.Empty;
                        string title = "Tài khoản đã được kích hoạt";
                        var path = Path.Combine(env.ContentRootPath, CoreContants.TEMPLATE_FOLDER_NAME, CoreContants.ACTIVE_PROVIDER_EMAIL_TEMPLATE_NAME);
                        using (StreamReader reader = new StreamReader(path))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{UserName}", item.FullName);
                        body = body.Replace("{PersonInCharge}", string.Join("hoặc", personInChargeOfProvider.Where(x => !string.IsNullOrEmpty(x.Name)).Select(i => i.Name)));
                        body = body.Replace("{Phone}", item.Phone);
                        //body = body.Replace("{UrlThumbnailMail}", "https://acp.monamedia.net/473e4f12-71b2-4a7b-8efb-2d7a953a1951-hinhnenmail.png");
                        //Thông báo email
                        await emailConfigurationService.Send(title, body.ToString(), tos);
                    }
                }
            }
            //Nếu là khách hàng update =>
            if (loginCheckCustomer && string.IsNullOrEmpty(itemModel.PersonInCharge))
            {
                if (itemModel.Id != userLoginCurrent.userId)
                    throw new AppException("Lỗi trong quá trình cập nhật!");
                item.PersonInCharge = providerOld.PersonInCharge;
                item.Username = providerOld.Username;
            }
            bool success = await this.domainService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult() { ResultCode = (int)HttpStatusCode.OK, ResultMessage = "Cập nhật nhà cung cấp thành công!", Success = true };
        }


        /// <summary>
        /// Danh sách nhà cung cấp
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách nhà cung cấp")]
        public override async Task<AppDomainResult> Get([FromQuery] ProviderSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_Providers> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<ProviderModel> pagedDataModel = mapper.Map<PagedList<ProviderModel>>(pagedData);
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Duyệt danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve-multiples-provider")]
        [AppAuthorize]
        [Description("Duyệt danh sách nhà cung cấp")]
        public virtual async Task<AppDomainResult> ApproveMultiples([FromBody] ProviderSearch baseSearch)
        {
            bool success = true;
            PagedList<ProviderModel> pagedListModel = new PagedList<ProviderModel>();
            PagedList<tbl_Providers> pagedData = await this.domainService.GetPagedListData(baseSearch);
            pagedListModel = mapper.Map<PagedList<ProviderModel>>(pagedData);
            if (pagedListModel.Items != null && pagedListModel.Items.Any())
            {
                var providers = await this.domainService.GetAsync(
                    e => !e.Deleted.Value && !e.Active.Value
                    && (pagedListModel.Items.Select(x => x.Id) == null || !pagedListModel.Items.Select(x => x.Id).Any()) || pagedListModel.Items.Select(x => x.Id).Contains(e.Id));


                foreach (var item in providers)
                {

                    item.Active = true;
                    Expression<Func<tbl_Providers, object>>[] includeProperties = new Expression<Func<tbl_Providers, object>>[]
                    {
                        e => e.Active
                    };
                    success &= await this.domainService.UpdateFieldAsync(item, includeProperties);
                }
            }
            else throw new AppException("Không có thông tin nhà cung cấp");
            return new AppDomainResult()
            {
                Success = success,
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Duyệt thành công!"
            };
        }


        [HttpPut("provider-update-onesignal-deviceid")]
        [AppAuthorize]
        [Description("Cập nhật id onesignal")]
        public virtual async Task<AppDomainResult> UpdateOneSignal_DeviceId(string oneSignal_DeviceId)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            if (string.IsNullOrEmpty(oneSignal_DeviceId))
                throw new AppException("Mã one signal là bắt buộc!");
            var userLogin = LoginContext.Instance.CurrentUser;
            var user = await domainService.GetByIdAsync(userLogin.userId);
            if (user == null)
                throw new AppException("Item không tồn tại!");
            user.OneSignal_DeviceId = oneSignal_DeviceId;
            Expression<Func<tbl_Providers, object>>[] includeProperties = new Expression<Func<tbl_Providers, object>>[]
            {
                e => e.OneSignal_DeviceId
            };
            await this.domainService.UpdateFieldAsync(user, includeProperties);

            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Cập nhật thành công!",
                Success = true
            };
        }
    }
}
