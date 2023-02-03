using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Biddings : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Mã gói thầu
        /// </summary>
        [StringLength(100)]
        [Description("Mã code")]
        public string Code { get; set; }
        /// <summary>
        /// Tên gói thầu
        /// </summary>
        [StringLength(500)]
        [Required]
        [Description("Tên")]
        public string Name { get; set; }
        /// <summary>
        /// Mô tả gói thầu
        /// </summary>
        [StringLength(1000)]
        [Description("Mô tả")]
        public string Description { get; set; }

        /// <summary>
        /// Id của sản phẩm
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Tên Sản phẩm
        /// </summary>
        [NotMapped]
        public string Product { get; set; }

        /// <summary>
        /// Tên người tạo
        /// </summary>
        [NotMapped]
        public string CreatedName { get; set; }
    }
}
