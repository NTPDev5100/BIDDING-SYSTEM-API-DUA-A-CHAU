using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models.Configuration
{
    /// <summary>
    /// Bảng câu hình template gửi đi
    /// </summary>
    public class SMSEmailTemplateModel : AppDomainModel
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
        public bool isSMS { get; set; }
        public string code { get; set; }

        public string name { get; set; }

        public string description { get; set; }
    }
}
