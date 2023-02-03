using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Request.Configuration
{
    /// <summary>
    /// Lịch sử OTP
    /// </summary>
    public class OTPHistoryRequest : DomainCreate
    {
        /// <summary>
        /// Thông tin người dùng
        /// </summary>
        public int? userId { get; set; }

        /// <summary>
        /// SDT gửi OTP
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Email gửi OTP
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Mã OTP
        /// </summary>
        public string otpValue { get; set; }

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? expiredDate { get; set; }

        [DefaultValue(false)]
        public bool isEmail { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int status { get; set; }
    }
}
