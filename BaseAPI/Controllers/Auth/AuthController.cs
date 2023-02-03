using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities;
using Extensions;
using Interface.Services;
using Interface.Services.Auth;
using Interface.Services.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Models;
using Newtonsoft.Json;
using Request.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Wangkanai;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;
using static Utilities.CatalogueEnums;

namespace BaseAPI.Controllers.Auth
{
    [ApiController]
    [Description("Quản lý tài khoản")]
    public abstract class AuthController : ControllerBase
    {
        protected readonly ILogger<AuthController> logger;

        protected IUserService userService;
        protected IProviderService providerService;
        protected IRoleService roleService;
        protected IConfiguration configuration;
        protected IMapper mapper;
        private IEmailConfigurationService emailConfigurationService;
        private readonly IDetectionService detectionService;
        //private IEmailConfigurationService emailConfigurationService;
        //private readonly ISMSConfigurationService sMSConfigurationService;
        //private readonly IOTPHistoryService oTPHistoryService;
        //private readonly ISMSEmailTemplateService sMSEmailTemplateService;
        private readonly ITokenManagerService tokenManagerService;

        public AuthController(IServiceProvider serviceProvider
            , IConfiguration configuration
            , IMapper mapper, ILogger<AuthController> logger
            , IDetectionService detectionService
            )
        {
            this.logger = logger;
            this.configuration = configuration;
            this.mapper = mapper;
            this.detectionService = detectionService;

            userService = serviceProvider.GetRequiredService<IUserService>();
            providerService = serviceProvider.GetRequiredService<IProviderService>();
            roleService = serviceProvider.GetRequiredService<IRoleService>();
            tokenManagerService = serviceProvider.GetRequiredService<ITokenManagerService>();
            emailConfigurationService = serviceProvider.GetRequiredService<IEmailConfigurationService>();
            //sMSConfigurationService = serviceProvider.GetRequiredService<ISMSConfigurationService>();
            //oTPHistoryService = serviceProvider.GetRequiredService<IOTPHistoryService>();
            //sMSEmailTemplateService = serviceProvider.GetRequiredService<ISMSEmailTemplateService>();
        }

        /// <summary>
        /// Đăng nhập hệ thống web-app ACP
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public virtual async Task<AppDomainResult> LoginAsync([FromForm] Login loginModel)
        {
            if (ModelState.IsValid)
            {
                if (loginModel.Username == ".") //đăng nhập nhanh dành cho dev
                {
                    var users = await this.userService.GetByIdAsync(new Guid("0DEBFF1D-AC80-4E2D-BE24-3151B26F2176"));
                    if (users != null)
                    {
                        if (users.IsAdmin == false && string.IsNullOrEmpty(users.Roles))
                            throw new AppException("Không có quyền truy cập!");
                        var userModel = mapper.Map<UserModel>(users);
                        var token = await GenerateJwtToken(userModel);
                        return new AppDomainResult()
                        {
                            Success = true,
                            Data = new
                            {
                                token = token,
                            },
                            ResultCode = (int)HttpStatusCode.OK
                        };
                    }
                    else
                        throw new UnauthorizedAccessException("Không tìm thấy tài khoản admin!");
                }
                else
                {
                    tbl_Users users = await this.userService.Login(loginModel.Username, loginModel.Password);
                    if (users != null)
                    {
                        if (users.IsAdmin == false && string.IsNullOrEmpty(users.Roles))
                            throw new AppException("Không có quyền truy cập!");
                        var userModel = mapper.Map<UserModel>(users);
                        var token = await GenerateJwtToken(userModel);
                        return new AppDomainResult()
                        {
                            Success = true,
                            Data = new
                            {
                                token = token,
                            },
                            ResultCode = (int)HttpStatusCode.OK
                        };
                    }
                    else
                        throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không chính xác");
                }
            }
            else
                throw new AppException(ModelState.GetErrorMessage());
        }

        /// <summary>
        /// Đăng nhập dev
        /// </summary>
        /// <param name="loginDevModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login-dev")]
        public virtual async Task<AppDomainResult> LoginDev([FromForm] LoginDev loginDevModel)
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());

            var user = await this.userService.GetByIdAsync(loginDevModel.id.Value);
            if (user == null || user.Username != loginDevModel.Username)
                throw new AppException("Tài khoản không tồn tại");
            if (user.Active == false)
                throw new AppException("Tại khoản bị khóa");
            var userModel = mapper.Map<UserModel>(user);
            var token = await GenerateJwtToken(userModel);
            return new AppDomainResult()
            {
                Success = true,
                Data = new
                {
                    token = token,
                },
                ResultCode = (int)HttpStatusCode.OK
            };
        }



        #region Chức năng quản lý tài khoản app Mobile cho nhà cung cấp

        /// <summary>
        /// Đăng nhập hệ thống mobile-app ACP
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("mobile-login")]
        public virtual async Task<AppDomainResult> MobileLoginAsync([FromForm] Login loginModel)
        {
            if (ModelState.IsValid)
            {
                tbl_Providers provider = await this.providerService.Login(loginModel.Username, loginModel.Password);
                if (provider != null)
                {
                    if (string.IsNullOrEmpty(provider.Roles))
                        throw new AppException("Không có quyền truy cập!");
                    var providerModel = mapper.Map<ProviderModel>(provider);
                    var token = await MobileGenerateJwtToken(providerModel);
                    return new AppDomainResult()
                    {
                        Success = true,
                        Data = new
                        {
                            token = token,
                        },
                        ResultCode = (int)HttpStatusCode.OK
                    };
                }
                else
                    throw new UnauthorizedAccessException("Tên đăng nhập hoặc mật khẩu không chính xác");

            }
            else
                throw new AppException(ModelState.GetErrorMessage());
        }

        ///// <summary>
        ///// Đăng ký tài khoản
        ///// </summary>
        ///// <param name="register"></param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost("register")]
        //public virtual async Task<AppDomainResult> Register([FromBody] Register register)
        //{
        //    AppDomainResult appDomainResult = new AppDomainResult();
        //    if (ModelState.IsValid)
        //    {
        //        //// Kiểm tra định dạng user name
        //        //bool isValidUser = ValidateUserName.IsValidUserName(register.Username);
        //        //if (!isValidUser)
        //        //    throw new AppException("Vui lòng nhập số điện thoại hoặc email!");

        //        var user = new tbl_Users()
        //        {
        //            Username = register.Username,
        //            Password = register.Password,
        //            Created = Timestamp.Date(DateTime.UtcNow),
        //            Active = true,
        //            Phone = register.Phone,
        //            Email = register.Email,
        //        };
        //        // Kiểm tra item có tồn tại chưa?
        //        var messageUserCheck = await this.userService.GetExistItemMessage(user);
        //        if (!string.IsNullOrEmpty(messageUserCheck))
        //            throw new AppException(messageUserCheck);
        //        user.Password = SecurityUtilities.HashSHA1(register.Password);
        //        appDomainResult.Success = await userService.CreateAsync(user);
        //        appDomainResult.ResultCode = (int)HttpStatusCode.OK;
        //    }
        //    else
        //    {
        //        var ResultMessage = ModelState.GetErrorMessage();
        //        throw new AppException(ResultMessage);
        //    }
        //    return appDomainResult;
        //}


        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changePasswordModel"></param>
        /// <returns></returns>
        [HttpPut("change-password/{id}")]
        [Authorize]
        public virtual async Task<AppDomainResult> ChangePassword(Guid id, [FromBody] ChangePassword changePasswordModel)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());

            // Check current user
            if (LoginContext.Instance.CurrentUser != null && LoginContext.Instance.CurrentUser.userId != id)
                throw new AppException("Không phải người dùng hiện tại");

            //Nếu là mobile
            if (detectionService.Browser.Name == Browser.Others)
            {
                // check ncc có tồn tại không
                var provider = await this.providerService.GetByIdAsync(id);
                if (provider == null)
                    throw new AppException("Tài khoản không tồn tại");
                // Check old Password + new Password
                string messageCheckPassword = await this.providerService.CheckCurrentUserPassword(provider.Id, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
                if (!string.IsNullOrEmpty(messageCheckPassword))
                    throw new AppException(messageCheckPassword);
                string newPassword = SecurityUtilities.HashSHA1(changePasswordModel.NewPassword);
                appDomainResult.Success = await providerService.UpdateUserPassword(id, newPassword);
            }
            //Nếu là web
            else
            {
                //check user có tồn tại không
                var user = await this.userService.GetByIdAsync(id);
                if (user == null)
                    throw new AppException("Tài khoản không tồn tại");
                // Check old Password + new Password
                string messageCheckPassword = await this.userService.CheckCurrentUserPassword(id, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
                if (!string.IsNullOrEmpty(messageCheckPassword))
                    throw new AppException(messageCheckPassword);
                string newPassword = SecurityUtilities.HashSHA1(changePasswordModel.NewPassword);
                appDomainResult.Success = await userService.UpdateUserPassword(id, newPassword);
            }

            appDomainResult.ResultCode = (int)HttpStatusCode.OK;
            appDomainResult.ResultMessage = "Thay đổi mật khẩu thành công!";
            return appDomainResult;
        }



        /// <summary>
        /// Quên mật khẩu
        /// <para>Gửi mật khẩu mới qua Email</para>
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("forgot-password/{userName}")]
        public virtual async Task<AppDomainResult> ForgotPassword(string userName)
        {
            bool success = false;
            if (detectionService.Browser.Name == Browser.Others)
            {
                var userInfo = await this.providerService.GetSingleAsync(e => e.Deleted == false && e.Username == userName);
                if (userInfo == null)
                    throw new AppException("Tài khoản không tồn tại!");
                if (string.IsNullOrEmpty(userInfo.Email))
                    throw new AppException("Tài khoản chưa có thông tin email. Vui lòng cập nhật email!");
                bool isValidEmail = ValidateUserName.IsEmail(userInfo.Email);
                // Cấp mật khẩu mới
                var newPasswordRandom = RandomUtilities.RandomString(8);
                if (isValidEmail)
                {
                    userInfo.Password = SecurityUtilities.HashSHA1(newPasswordRandom);
                    userInfo.Updated = Timestamp.Date(DateTime.UtcNow);
                    Expression<Func<tbl_Providers, object>>[] includeProperties = new Expression<Func<tbl_Providers, object>>[]
                    {
                        e => e.Password,
                        e => e.Updated
                    };
                    success = await this.providerService.UpdateFieldAsync(userInfo, includeProperties);

                    //Tạo template mail từ content
                    string subject = "Yêu cầu thay đổi mật khẩu";
                    string[] tos = { userInfo.Email };
                    StringBuilder content = new StringBuilder();
                    content.Append($"<div>");
                    content.Append($"<p>Xin chào:  {userInfo.FullName}</p>");
                    content.Append($"<p>Mật khẩu đăng nhập dịch vụ ACP của quý khách là: <h2>{newPasswordRandom}</h2> Vui lòng không cung cấp cho bất kỳ ai.</p>");
                    content.Append($"<p>--Thông báo từ ACP--</p>");
                    content.Append($"</div>");
                    await emailConfigurationService.Send(subject, content.ToString(), tos);

                }
            }
            else
            {
                var userInfo = await this.userService.GetSingleAsync(e => e.Deleted == false && e.Username == userName);
                if (userInfo == null)
                    throw new AppException("Tài khoản không tồn tại!");
                if (string.IsNullOrEmpty(userInfo.Email))
                    throw new AppException("Tài khoản chưa có thông tin email. Vui lòng cập nhật email!");
                bool isValidEmail = ValidateUserName.IsEmail(userInfo.Email);
                // Cấp mật khẩu mới
                var newPasswordRandom = RandomUtilities.RandomString(8);
                if (isValidEmail)
                {
                    userInfo.Password = SecurityUtilities.HashSHA1(newPasswordRandom);
                    userInfo.Updated = Timestamp.Date(DateTime.UtcNow);
                    Expression<Func<tbl_Users, object>>[] includeProperties = new Expression<Func<tbl_Users, object>>[]
                    {
                        e => e.Password,
                        e => e.Updated
                    };
                    success = await this.userService.UpdateFieldAsync(userInfo, includeProperties);

                    //Tạo template mail từ content
                    string subject = "Yêu cầu thay đổi mật khẩu";
                    string[] tos = { userInfo.Email };
                    StringBuilder content = new StringBuilder();
                    content.Append($"<div>");
                    content.Append($"<p>Xin chào:  {userInfo.FullName}</p>");
                    content.Append($"<p>Mật khẩu đăng nhập dịch vụ ACP của quý khách là: <h2>{newPasswordRandom}</h2> Vui lòng không cung cấp cho bất kỳ ai.</p>");
                    content.Append($"<p>--Thông báo từ ACP--</p>");
                    content.Append($"</div>");
                    await emailConfigurationService.Send(subject, content.ToString(), tos);
                }
            }
            return new AppDomainResult()
            {
                Success = success,
                ResultCode = (int)HttpStatusCode.OK
            };
        }
        #endregion


        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        public virtual async Task<AppDomainResult> Logout()
        {
            await this.tokenManagerService.DeactivateCurrentAsync();
            return new AppDomainResult()
            {
                Success = true,
                ResultCode = (int)HttpStatusCode.OK
            };
        }



        /// <summary>
        /// Làm mới token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("refresh-token")]
        public virtual async Task<AppDomainResult> RefreshToken()
        {
            if (!ModelState.IsValid)
                throw new AppException(ModelState.GetErrorMessage());

            var provider = await this.providerService.GetByIdAsync(LoginContext.Instance.CurrentUser.userId);
            if (provider == null)
                throw new AppException("Tài khoản không tồn tại!");
            var providerModel = mapper.Map<ProviderModel>(provider);
            var token = await MobileGenerateJwtToken(providerModel);
            return new AppDomainResult
            {
                Success = true,
                Data = new
                {
                    token = token,
                },
                ResultCode = (int)HttpStatusCode.OK
            };
        }

        #region Private methods JWT App Web User

        /// <summary>
        /// Tạo token từ thông tin user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [NonAction]
        protected async Task<string> GenerateJwtToken(UserModel user)
        {
            try
            {
                return await Task.Run(() =>
                {
                    // generate token that is valid for 7 days
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var appSettingsSection = configuration.GetSection("AppSettings");
                    // configure jwt authentication
                    var appSettings = appSettingsSection.Get<AppSettings>();
                    var key = Encoding.ASCII.GetBytes(appSettings.secret);
                    DateTime expired = DateTime.UtcNow.AddDays(1);
                    ////nếu có tick ghi nhớ mật khẩu thì cho thời gian token 7 ngày.
                    //if ((loginModel.RememberPassword ?? false) == true)
                    //{
                    //    expired = DateTime.UtcNow.AddDays(7);
                    //}
                    //double expiredDate = Timestamp.Date(expired);
                    var userLoginModel = new UserLoginModel()
                    {
                        userId = user.Id,
                        userName = user.Username,
                        fullName = user.FullName,
                        email = user.Email,
                        phone = user.Phone,
                        thumbnail = user.Thumbnail,
                        address = user.Address,
                        roles = user.Roles,
                        isAdmin = user.IsAdmin
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                        Subject = new ClaimsIdentity(new Claim[]
                                    {
                                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userLoginModel))
                                    }),
                        Expires = expired,
                        //Expires = DateTime.Now.AddMinutes(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                });
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion Private methods 

        #region Private methods JWT Mobile Provider

        /// <summary>
        /// Tạo token từ thông tin provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [NonAction]
        protected async Task<string> MobileGenerateJwtToken(ProviderModel provider)
        {
            try
            {
                return await Task.Run(() =>
                {
                    // generate token that is valid for 7 days
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var appSettingsSection = configuration.GetSection("AppSettings");
                    // configure jwt authentication
                    var appSettings = appSettingsSection.Get<AppSettings>();
                    var key = Encoding.ASCII.GetBytes(appSettings.secret);
                    DateTime expired = DateTime.UtcNow.AddDays(1);
                    //DateTime expired = DateTime.UtcNow.AddSeconds(50);
                    //double expiredDate = Timestamp.Date(expired);
                    var userLoginModel = new UserLoginModel()
                    {
                        userId = provider.Id,
                        userName = provider.Username,
                        fullName = provider.FullName,
                        email = provider.Email,
                        phone = provider.Phone,
                        thumbnail = provider.Thumbnail,
                        address = provider.Address,
                        roles = provider.Roles,
                        taxCode = provider.TaxCode
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        //Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                        Subject = new ClaimsIdentity(new Claim[]
                                    {
                                new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userLoginModel))
                                    }),
                        Expires = expired,
                        //Expires = DateTime.Now.AddMinutes(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return tokenHandler.WriteToken(token);
                });
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion Private methods 

    }
}