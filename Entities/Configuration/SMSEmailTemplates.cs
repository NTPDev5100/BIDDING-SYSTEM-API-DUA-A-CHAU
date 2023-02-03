using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Configuration
{
    public class SMSEmailTemplates : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Mẫu là SMS
        /// </summary>
        public bool IsSMS { get; set; }
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(500)]
        [Required]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
