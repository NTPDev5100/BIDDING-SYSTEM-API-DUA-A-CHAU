using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CatalogueEnums;

namespace Models
{
    public class TechnicalProductModel : DomainModels.AppDomainModel
    {
        /// <summary>
        /// Id sản phẩm
        /// </summary>
        public Guid? ProductId { get; set; }

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
