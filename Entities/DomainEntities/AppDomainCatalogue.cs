using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DomainEntities
{
    public class AppDomainCatalogue : DomainEntities
    {
        [StringLength(50)]
        [Description("Mã code")]
        public string Code { get; set; }
        [StringLength(500)]
        [Required]
        [Description("Tên")]
        public string Name { get; set; }
        [StringLength(1000)]
        [Description("Mô tả")]
        public string Description { get; set; }
    }
}
