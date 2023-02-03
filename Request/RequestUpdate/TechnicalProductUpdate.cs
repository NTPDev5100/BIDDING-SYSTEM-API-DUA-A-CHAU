using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class TechnicalProductUpdate : DomainRequests.DomainUpdate
    {
        /// <summary>
        /// Nội dung kỹ thuật
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập kỹ thuật cho sản phẩm!")]
        public string TechnicalValue { get; set; }
    }
}
