using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Catalogue.CatalogueUpdate
{
    public class ProductsUpdate : RequestCatalogueUpdateModel
    {
        public string Thumbnail { get; set; }
        /// <summary>
        /// Nội dung kỹ thuật
        /// </summary>
        public string TechnicalValue { get; set; }
    }
}
