using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class TechnicalProductCreate : DomainRequests.DomainCreate
    {
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        [Required]
        public Guid? ProductId { get; set; }

        /// <summary>
        /// Nội dung kỹ thuật
        /// </summary>
        [Required(ErrorMessage ="Vui lòng nhập kỹ thuật cho sản phẩm!")]
        public string TechnicalValue { get; set; }
    }
}
