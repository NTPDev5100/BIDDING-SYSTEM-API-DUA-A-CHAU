using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Entities.Configuration
{
    /// <summary>
    /// Lịch sử OTP
    /// </summary>
    public class OTPHistories : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// SDT gửi OTP
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email gửi OTP
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Mã OTP
        /// </summary>
        public string OtpValue { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public double? ExpiredDate { get; set; }

        [DefaultValue(false)]
        public bool IsEmail { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
    }
}
