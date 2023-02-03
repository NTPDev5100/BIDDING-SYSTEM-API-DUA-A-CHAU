using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_TechnicalProduct : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        [Description("Id sản phẩm")]
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Nội dung kỹ thuật
        /// </summary>
        [Description("Nội dung kỹ thuật")]
        public string TechnicalValue { get; set; }

        //[NotMapped]
        //public string TechnicalOptionName { get; set; }
    }
}
