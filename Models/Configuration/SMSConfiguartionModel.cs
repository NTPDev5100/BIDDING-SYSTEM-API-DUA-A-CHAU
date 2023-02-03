using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Configuration
{
    public class SMSConfiguartionModel : AppDomainModel
    {
        public string apiKey { get; set; }

        public string secretKey { get; set; }

        public string brandName { get; set; }

        public int smsType { get; set; }

        /// <summary>
        /// Cú pháp tin nhắn mẫu
        /// </summary>
        public string templateText { get; set; }

        /// <summary>
        /// Url web service
        /// </summary>
        public string webServiceUrl { get; set; }
    }
}
