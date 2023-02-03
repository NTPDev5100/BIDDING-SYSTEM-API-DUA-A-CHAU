using Request.DomainRequests;
using Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Request.Configuration
{
    /// <summary>
    /// Cấu hình email
    /// </summary>
    public class EmailConfigurationRequest : DomainCreate
    {
        /// <summary>
        /// SMTP Server
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập SMTP Server!")]
        [MaxLength(1000, ErrorMessage = "Giá trị smtp server phải nhỏ hơn 1000 kí tự!")]
        public string smtpServer { set; get; }

        /// <summary>
        /// Port 
        /// </summary>
        [Required]
        public int port { set; get; }

        /// <summary>
        /// Cờ mở SSL
        /// </summary>
        [Required]
        public bool enableSsl { set; get; }

        /// <summary>
        /// Loại connect
        /// </summary>
        [Required]
        public int connectType { set; get; }

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên hiển thị!")]
        [MaxLength(1000, ErrorMessage = "Tên hiển thị phải nhỏ hơn 1000 kí tự")]
        public string displayName { set; get; }

        /// <summary>
        /// Tên đăng nhập cấu hình
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập UserName!")]
        [MaxLength(1000, ErrorMessage = "Tên hiển thị phải nhỏ hơn 1000 kí tự")]
        public string userName { set; get; }

        [MaxLength(1000)]
        public string email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [StringLength(128, ErrorMessage = "Mật khẩu phải ít nhất 8 kí tự", MinimumLength = 8)]
        public string password { set; get; }

        public int itemSendCount { get; set; }

        public int timeSend { get; set; }

        public void EncryptPassword()
        {
            password = StringCipher.Encrypt(password, StringCipher.PassPhrase);
        }
    }
}
