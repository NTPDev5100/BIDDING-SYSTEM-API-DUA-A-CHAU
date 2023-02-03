using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Request.Configuration
{
    public class SMSConfiguartionRequest : DomainCreate
    {
        [Required(ErrorMessage = "Vui lòng nhập API Key")]
        [DataType(DataType.Password)]
        public string apiKey { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Secret Key")]
        [DataType(DataType.Password)]
        public string secretKey { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Brand name")]
        public string brandName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Loại SMS")]
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
