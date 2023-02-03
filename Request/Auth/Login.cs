using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Request.Auth
{
    public class Login
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập")]
        public string Username { set; get; }
        /// <summary>
        /// Mật khẩu
        /// </summary>

        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập")]
        public string Password { set; get; }
        /// <summary>
        /// Ghi nhớ mật khẩu
        /// </summary>
        public bool? RememberPassword { get; set; }

        //public string OTPValue { get; set; }
    }
    public class LoginDev
    {
        public string Username { set; get; }
        public Guid? id { get; set; }
    }
}
