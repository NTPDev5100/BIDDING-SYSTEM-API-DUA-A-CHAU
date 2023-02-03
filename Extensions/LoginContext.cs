using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Extensions
{
    public sealed class LoginContext
    {
        private static LoginContext instance = null;

        private LoginContext()
        {
        }

        public static LoginContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LoginContext();
                }
                return instance;
            }
        }

        public UserLoginModel CurrentUser
        {
            get
            {
                if(HttpContext.Current != null)
                {
                    var user = (UserLoginModel)Extensions.HttpContext.Current.Items["User"];
                    if (user != null)
                        return user;
                }               
                return null;
            }
        }

        public void Clear()
        {
            instance = null;
        }

        public UserLoginModel GetCurrentUser(IHttpContextAccessor httpContext)
        {
            if (httpContext != null && httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claim = httpContext.HttpContext.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.UserData);
                if (claim != null)
                    return JsonConvert.DeserializeObject<UserLoginModel>(claim.Value);
            }
            return null;
        }
    }

    public class UserLoginModel
    {
        public Guid userId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string thumbnail { get; set; }
        /// <summary>
        /// Danh sách tính năng được truy cập
        /// </summary>
        public string permission { get; set; }
        public string address { get; set; }
        /// <summary>
        /// Danh sách menu được truy cập - nhìn thấy
        /// </summary>
        public string menuList { get; set; }
        public string roles { get; set; }
        /// <summary>
        /// Thời gian hết hạn của token
        /// </summary>
        //public double expiredDate { get; set; }
        public bool isAdmin { get; set; }
        public string taxCode { get; set; }
    }
}
