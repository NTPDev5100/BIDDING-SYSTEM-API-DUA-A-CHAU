using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Request.Configuration
{
    /// <summary>
    /// Bảng câu hình template gửi đi
    /// </summary>
    public class SMSEmailTemplateRequest : DomainCreate
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// Mẫu là SMS
        /// </summary>
        [DefaultValue(false)]
        public bool isSMS { get; set; }
    }
}
