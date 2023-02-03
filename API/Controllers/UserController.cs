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
using Newtonsoft.Json;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace BaseAPI.Controllers
{
    /// <summary>
    /// Quản lý nhân viên
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Description("Quản lý nhân viên")]
    [Authorize]
    public class UserController : BaseController<tbl_Users, UserModel, UserCreate, UserUpdate, UserSearch>
    {
        private IRoleService roleService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(IServiceProvider serviceProvider, ILogger<BaseController<tbl_Users, UserModel, UserCreate, UserUpdate, UserSearch>> logger
            , IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(serviceProvider, logger, env)
        {
            this.domainService = serviceProvider.GetRequiredService<IUserService>();
            this.roleService = serviceProvider.GetRequiredService<IRoleService>();
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize]
        [Description("Thêm mới nhân viên")]
        public override async Task<AppDomainResult> AddItem([FromBody] UserCreate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());

            var item = mapper.Map<tbl_Users>(itemModel);
            if (item == null)
                throw new AppException("Item không tồn tại");

            Expression<Func<tbl_Users, tbl_Users>> includeProperties = e => new tbl_Users() { Code = e.Code, Username = e.Username, Phone = e.Phone, Email = e.Email };
            IList<tbl_Users> users = await this.domainService.GetAsync(new Expression<Func<tbl_Users, bool>>[] { }, includeProperties);
            if (users.Any(x => x.Phone == item.Phone))
                throw new AppException("Số điện thoại đã tồn tại trong hệ thống!");
            if (users.Any(x => x.Username == item.Username))
                throw new AppException("Tên đăng nhập đã tồn tại trong hệ thống!");
            if (users.Any(x => x.Email == item.Email))
                throw new AppException("Email đã tồn tại trong hệ thống!");

            string code = "NV" + RandomUtilities.RandomNumber(8);
            while (users.Any(x => x.Code == code))
            {
                code = "NV" + RandomUtilities.RandomNumber(8);
            }

            item.Code = code;
            item.Password = /*string.IsNullOrEmpty(item.Password) ? SecurityUtilities.HashSHA1("23312331") :*/ SecurityUtilities.HashSHA1(item.Password);
            item.Username = (item.Username).Replace(" ", "").Trim();
            if (string.IsNullOrEmpty(itemModel.Roles))
            {
                return new AppDomainResult() { ResultCode = (int)HttpStatusCode.BadRequest, ResultMessage = "Vui lòng chọn chức vụ cho nhân viên!", Success = false };
            }
            List<ObjectJsonRole> roleOfUsers = JsonConvert.DeserializeObject<List<ObjectJsonRole>>(itemModel.Roles);
            List<ObjectJsonRole> roleInUsers = new List<ObjectJsonRole>();
            foreach (ObjectJsonRole itemRole in roleOfUsers)
            {
                var role = await roleService.GetByIdAsync(itemRole.Id);
                if (role == null)
                    throw new AppException("Quyền không tồn tại");
                roleInUsers.Add(new ObjectJsonRole { Id = role.Id, Name = role.Name, RoleNumberLevel = role.RoleNumberLevel.Value });
            }
            item.Roles = JsonConvert.SerializeObject(roleInUsers);

            bool success = await this.domainService.CreateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult()
            {
                ResultCode = (int)HttpStatusCode.OK,
                ResultMessage = "Thêm mới nhân viên thành công!",
                Success = true
            };
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="itemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [AppAuthorize]
        [Description("Cập nhật nhân viên")]
        public override async Task<AppDomainResult> UpdateItem([FromBody] UserUpdate itemModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());

            var item = mapper.Map<tbl_Users>(itemModel);
            if (item == null)
                throw new KeyNotFoundException("Item không tồn tại");
            Expression<Func<tbl_Users, tbl_Users>> includeProperties = e => new tbl_Users() { Code = e.Code, Username = e.Username, Phone = e.Phone, Email = e.Email, Id = e.Id };
            IList<tbl_Users> users = await this.domainService.GetAsync(new Expression<Func<tbl_Users, bool>>[] { }, includeProperties);
            if (users.Any(x => x.Phone == item.Phone && x.Id != item.Id))
                throw new AppException("Số điện thoại đã tồn tại trong hệ thống!");
            if (users.Any(x => x.Username == item.Username && x.Id != item.Id))
                throw new AppException("Tên đăng nhập đã tồn tại trong hệ thống!");
            if (users.Any(x => x.Email == item.Email && x.Id != item.Id))
                throw new AppException("Email đã tồn tại trong hệ thống!");
            if (!string.IsNullOrEmpty(itemModel.Password))
            {
                if (itemModel.Password.Length < 8)
                    throw new AppException("Mật khẩu phải lớn hơn 8 kí tự");
                item.Password = SecurityUtilities.HashSHA1(item.Password);
            }
            if (string.IsNullOrEmpty(itemModel.Password))
            {
                var userOld = await domainService.GetByIdAsync(item.Id);
                if (userOld != null)
                    item.Password = userOld.Password;
            }
            item.Username = (item.Username).Replace(" ", "").Trim();
            if (!string.IsNullOrEmpty(itemModel.Roles))
            {
                List<ObjectJsonRole> roleOfUsers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObjectJsonRole>>(itemModel.Roles);
                List<ObjectJsonRole> roleInUsers = new List<ObjectJsonRole>();
                foreach (ObjectJsonRole itemRole in roleOfUsers)
                {
                    var role = await roleService.GetByIdAsync(itemRole.Id);
                    if (role == null)
                        throw new KeyNotFoundException("Quyền không tồn tại");
                    roleInUsers.Add(new ObjectJsonRole { Id = role.Id, Name = role.Name, RoleNumberLevel = role.RoleNumberLevel.Value });
                }
                item.Roles = JsonConvert.SerializeObject(roleInUsers);
            }

            bool success = await this.domainService.UpdateAsync(item);
            if (!success)
                throw new Exception("Lỗi trong quá trình xử lý");
            return new AppDomainResult() { ResultCode = (int)HttpStatusCode.OK, ResultMessage = "Cập nhật nhân viên thành công!", Success = true };
        }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet]
        [AppAuthorize]
        [Description("Danh sách nhân viên")]
        public override async Task<AppDomainResult> Get([FromQuery] UserSearch baseSearch)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());
            PagedList<tbl_Users> pagedData = await this.domainService.GetPagedListData(baseSearch);
            PagedList<UserModel> pagedDataModel = mapper.Map<PagedList<UserModel>>(pagedData);
            return new AppDomainResult
            {
                Data = pagedDataModel,
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPut("user-update-onesignal-deviceid")]
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
            Expression<Func<tbl_Users, object>>[] includeProperties = new Expression<Func<tbl_Users, object>>[]
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