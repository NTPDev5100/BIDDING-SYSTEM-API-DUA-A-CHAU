using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Catalogue
{
    public class tbl_Menu : AppDomainCatalogue
    {
        /// <summary>
        /// Icon của menu
        /// </summary>
        [Description("Icon của menu")]
        [StringLength(1000, ErrorMessage = "Số kí tự của Icon phải nhỏ hơn 1000!")]
        public string Icon { get; set; }
        /// <summary>
        /// Đường dẫn đến trang
        /// </summary>
        [Description("Đường dẫn đến trang")]
        [StringLength(500, ErrorMessage = "Số kí tự của Link phải nhỏ hơn 500!")]
        public string Link { get; set; }
    }
}
