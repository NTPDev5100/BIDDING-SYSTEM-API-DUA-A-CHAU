using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.DomainRequests
{
    public class RequestCatalogueUpdateModel : DomainUpdate
    {
        [StringLength(50, ErrorMessage = "Vui lòng nhập Mã nhỏ hơn 50 kí tự")]
        public string Code { get; set; }
        [StringLength(1000, ErrorMessage = "Vui lòng nhập Tên nhỏ hơn 1000 kí tự")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
