using AutoMapper;
using Entities;
using Entities.Search;
using Extensions;
using Interface.DbContext;
using Interface.Services;
using Interface.Services.Configuration;
using Interface.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace Service.Services
{
    public class ProviderService : DomainService<tbl_Providers, ProviderSearch>, IProviderService
    {
        protected IAppDbContext coreDbContext;
        private IEmailConfigurationService emailConfigurationService;
        private IUserService userService;
        public ProviderService(IUserService userService, IEmailConfigurationService emailConfigurationService, IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
            this.emailConfigurationService = emailConfigurationService;
            this.userService = userService;
        }
        protected override string GetStoreProcName()
        {
            return "Provider_GetPagingData";
        }

        /// <summary>
        /// Kiểm tra provider đăng nhập
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<tbl_Providers> Login(string userName, string password)
        {

            tbl_Providers providers = await Queryable.Where(e => e.Deleted == false && (e.Username == userName || e.Phone == userName || e.Email == userName)).FirstOrDefaultAsync();
            if (providers != null)
            {
                if (providers.Active == false)
                {
                    throw new Exception("Tài khoản chưa được kích hoạt");
                }
                if (providers.Password == SecurityUtilities.HashSHA1(password))
                {
                    return providers;
                }
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra pass word cũ đã giống chưa
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> CheckCurrentUserPassword(Guid providerId, string password, string newPasssword)
        {
            string message = string.Empty;
            List<string> messages = new List<string>();
            bool isCurrentPassword = await this.Queryable.AnyAsync(x => x.Id == providerId && x.Password == SecurityUtilities.HashSHA1(password));
            bool isDuplicateNewPassword = await this.Queryable.AnyAsync(x => x.Id == providerId && x.Password == SecurityUtilities.HashSHA1(newPasssword));
            if (!isCurrentPassword)
                messages.Add("Mật khẩu cũ không chính xác");
            else if (isDuplicateNewPassword)
                messages.Add("Mật khẩu mới không được trùng mật khẩu cũ");
            if (messages.Any())
                message = string.Join("; ", messages);
            return message;
        }


        /// <summary>
        /// Cập nhật password mới cho provider
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPassword(Guid providerId, string newPassword)
        {
            bool result = false;

            var existProviderInfo = await this.unitOfWork.Repository<tbl_Providers>().GetQueryable().Where(e => e.Id == providerId).FirstOrDefaultAsync();
            if (existProviderInfo != null)
            {
                existProviderInfo.Password = newPassword;
                existProviderInfo.Updated = Timestamp.UtcNow();
                Expression<Func<tbl_Providers, object>>[] includeProperties = new Expression<Func<tbl_Providers, object>>[]
                {
                    e => e.Password,
                    e => e.Updated
                };
                await this.unitOfWork.Repository<tbl_Providers>().UpdateFieldsSaveAsync(existProviderInfo, includeProperties);
                await this.unitOfWork.SaveAsync();
                result = true;
            }

            return result;
        }


        /// <summary>
        /// Kiểm tra nhà cung cấp đã tồn tại chưa
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<string> GetExistItemMessage(tbl_Providers item)
        {
            List<string> message = new List<string>();
            string result = string.Empty;
            bool isExistEmail = !string.IsNullOrEmpty(item.Email) && await Queryable.AnyAsync(x => x.Deleted == false && x.Id != item.Id && x.Email == item.Email);
            bool isExistPhone = !string.IsNullOrEmpty(item.Phone) && await Queryable.AnyAsync(x => x.Deleted == false && x.Id != item.Id && x.Phone == item.Phone);
            bool isExistUserName = !string.IsNullOrEmpty(item.Username) && await Queryable.AnyAsync(x => x.Deleted == false && x.Id != item.Id && x.Username == item.Username);

            if (isExistEmail)
                message.Add("Email đã tồn tại!");
            if (isExistPhone)
                message.Add("Số điện thoại đã tồn tại!");
            if (isExistUserName)
                message.Add("Tên đăng nhập đã tồn tại!");
            if (message.Any())
                result = string.Join(" ", message);
            return result;
        }

        public async Task AccountActivationNotice (Guid providerId ,bool isActive, string personInCharge, string email)
        {
            var provider = await this.Queryable.Where(x => x.Id == providerId).AsNoTracking().FirstOrDefaultAsync();
            if(provider.Active == false)
            {
                if (isActive == true)
                {
                    List<ObjectJsonCustom> personInChargeOfProvider = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(personInCharge);
                    string subject = "Chào mừng bạn gia nhập ACP";
                    if (!string.IsNullOrEmpty(email))
                    {
                        // Gửi email
                        string[] tos = new string[] {email };
                        if (tos.Any())
                        {
                            StringBuilder contentEmail = new StringBuilder();
                            contentEmail.Append($"<div>");
                            contentEmail.Append($"<h2>{provider.FullName} thân mến,</h2>");
                            contentEmail.Append($"<p>Tài khoản của bạn trên hệ thống ACP đã được kích hoạt thành công.</p>");
                            contentEmail.Append($"<p>Xin vui lòng liên hệ nhân viên phụ trách: {personInChargeOfProvider.Select(x => x.Name).FirstOrDefault()} để biết thêm thông tin về tài khoản.</p>");
                            contentEmail.Append($"<p>--Thông báo từ hệ thống ACP--</p>");
                            contentEmail.Append($"</div>");
                            await emailConfigurationService.Send
                            (
                                subject,
                                contentEmail.ToString(),
                                tos
                            );
                        }
                    }                   
                }
            }       
        }

        public async Task<string> GetPersonCharge(string personChargeStr)
        {
            string personCharge = string.Empty;
            if (!string.IsNullOrEmpty(personChargeStr))
            {
                List<ObjectJsonCustom> personInChargeOfProvider = JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(personChargeStr);
                List<ObjectJsonCustom> personInChargeInProvider = new List<ObjectJsonCustom>();
                foreach (ObjectJsonCustom itemPersonInCharge in personInChargeOfProvider)
                {
                    var person = await userService.GetByIdAsync(itemPersonInCharge.Id);
                    if (person == null)
                        return personCharge;
                    personInChargeInProvider.Add(new ObjectJsonCustom { Id = person.Id, Name = person.FullName });
                }
                personCharge = JsonConvert.SerializeObject(personInChargeInProvider);
            }
            else
            {
                List<ObjectJsonCustom> personInChargeInProvider = new List<ObjectJsonCustom>();
                personInChargeInProvider.Add(new ObjectJsonCustom { Id = LoginContext.Instance.CurrentUser.userId, Name = LoginContext.Instance.CurrentUser.fullName });
                personCharge = JsonConvert.SerializeObject(personInChargeInProvider);
            }
            return personCharge;
        }




        /// <summary>
        /// Cập nhật thông tin provider token
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="token"></param>
        /// <param name="isLogin"></param>
        /// <returns></returns>
        //public async Task<bool> UpdateProviderToken(Guid providerId, string token, bool isLogin = false)
        //{
        //    bool result = false;

        //    var providerInfo = await this.unitOfWork.Repository<tbl_Providers>().GetQueryable().Where(e => e.Id == providerId).FirstOrDefaultAsync();
        //    if (providerInfo != null)
        //    {
        //        if (isLogin)
        //        {
        //            providerInfo.Token = token;
        //            providerInfo.ExpiredDate = Timestamp.Date(DateTime.UtcNow.AddDays(1));
        //        }
        //        else
        //        {
        //            providerInfo.Token = string.Empty;
        //            providerInfo.ExpiredDate = null;
        //        }
        //        Expression<Func<tbl_Providers, object>>[] includeProperties = new Expression<Func<tbl_Providers, object>>[]
        //        {
        //            e => e.Token,
        //            e => e.ExpiredDate
        //        };
        //        this.unitOfWork.Repository<tbl_Providers>().UpdateFieldsSave(providerInfo, includeProperties);
        //        await this.unitOfWork.SaveAsync();
        //        result = true;
        //    }

        //    return result;
        //}

    }
}
