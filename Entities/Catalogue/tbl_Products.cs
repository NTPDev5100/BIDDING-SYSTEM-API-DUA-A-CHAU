using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Products : DomainEntities.AppDomainCatalogue
    {

        /// <summary>
        /// Hình sản phẩm
        /// </summary>
        public string Thumbnail { get; set; }

    }
}
