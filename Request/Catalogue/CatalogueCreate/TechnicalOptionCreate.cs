using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Catalogue.CatalogueCreate
{
    public class TechnicalOptionCreate : DomainRequests.RequestCatalogueCreateModel
    {

        /// <summary>
        /// Định dạng file
        /// </summary>
        [Required(ErrorMessage ="Vui lòng chọn định dạng cho loại kỹ thuật!")]
        public bool isFile { get; set; }
    }
}
