using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Catalogue.CatalogueUpdate
{
    public class TechnicalOptionUpdate : DomainRequests.RequestCatalogueUpdateModel
    {
        /// <summary>
        /// Định dạng file
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn định dạng cho loại kỹ thuật!")]
        public bool isFile { get; set; }
    }
}
