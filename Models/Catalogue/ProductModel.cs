using Entities;
using Models.DomainModels;
using Newtonsoft.Json;
using Request.Catalogue.CatalogueCreate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CatalogueEnums;

namespace Models
{
    public class ProductModel : AppDomainCatalogueModel
    {
        public string Thumbnail { get; set; }

        public string CreatedName { get; set; }
        /// <summary>
        /// Nội dung kỹ thuật
        /// </summary>
        public string TechnicalValue { get; set; }
        public List<ObjectTechnicalProduct> TechnicalValueList
        {
            get
            {

                try
                {
                    return JsonConvert.DeserializeObject<List<ObjectTechnicalProduct>>(TechnicalValue);
                }
                catch { return null; }

            }
        }
    }
}
