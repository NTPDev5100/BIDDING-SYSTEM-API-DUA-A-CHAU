using Models.DomainModels;
using Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Configuration
{
    /// <summary>
    /// Cấu hình email
    /// </summary>
    public class EmailConfigurationModel : AppDomainModel
    {
        /// <summary>
        /// SMTP Server
        /// </summary>
        public string smtpServer { set; get; }

        /// <summary>
        /// Port 
        /// </summary>
        public int port { set; get; }

        /// <summary>
        /// Cờ mở SSL
        /// </summary>
        public bool enableSsl { set; get; }

        /// <summary>
        /// Loại connect
        /// </summary>
        public int connectType { set; get; }

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string displayName { set; get; }

        /// <summary>
        /// Tên đăng nhập cấu hình
        /// </summary>
        public string username { set; get; }

        public string email { get; set; }

        public string password { set; get; }

        public int itemSendCount { get; set; }

        public int timeSend { get; set; }

        public void EncryptPassword()
        {
            password = StringCipher.Encrypt(password, StringCipher.PassPhrase);
        }
    }
}
