using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Catalogue
{
    public class TechnicalOptionModel : AppDomainCatalogueModel
    {
        /// <summary>
        /// Định dạng file
        /// </summary>
        public bool isFile { get; set; }
    }
}
