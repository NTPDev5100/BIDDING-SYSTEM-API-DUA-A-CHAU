using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    /// <summary>
    /// Cấu hình SMS
    /// </summary>
    public class tbl_SMSConfigurations : DomainEntities.DomainEntities
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BrandName { get; set; }
        public int SmsType { get; set; }

        /// <summary>
        /// Cú pháp tin nhắn mẫu
        /// </summary>
        public string TemplateText { get; set; }

        /// <summary>
        /// Url web service
        /// </summary>
        public string WebServiceUrl { get; set; }
    }
}
