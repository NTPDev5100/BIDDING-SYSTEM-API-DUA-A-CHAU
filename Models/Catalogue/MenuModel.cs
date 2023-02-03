using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Catalogue
{
    public class MenuModel : AppDomainCatalogueModel
    {
        /// <summary>
        /// Icon của menu
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Đường dẫn đến trang
        /// </summary>
        public string Link { get; set; }
    }
}
