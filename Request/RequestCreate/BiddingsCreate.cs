using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class BiddingsCreate : DomainRequests.DomainCreate
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [StringLength(500, ErrorMessage = "Vui lòng nhập Tên nhỏ hơn 500 kí tự")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Vui lòng nhập Tên nhỏ hơn 500 kí tự")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Vui lòng chọn sản phẩm!")]
        public string Product { get; set; }
    }
}
