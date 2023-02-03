using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Catalogue.CatalogueCreate
{
    public class RequestMenuCreateModel : RequestCatalogueCreateModel
    {
        /// <summary>
        /// Icon của menu
        /// </summary>
        [StringLength(1000, ErrorMessage = "Số kí tự của Icon phải nhỏ hơn 1000!")]
        public string Icon { get; set; }
        /// <summary>
        /// Đường dẫn đến trang
        /// </summary>
        [StringLength(500, ErrorMessage = "Số kí tự của Link phải nhỏ hơn 500!")]
        public string Link { get; set; }
    }
}
