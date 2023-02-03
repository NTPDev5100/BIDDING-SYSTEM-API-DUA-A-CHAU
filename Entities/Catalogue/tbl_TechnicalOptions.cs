using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Catalogue
{
    public class tbl_TechnicalOptions : DomainEntities.AppDomainCatalogue
    {
        /// <summary>
        /// Định dạng file
        /// </summary>
        [Description("Định dạng file")]
        public bool isFile { get; set; }
    }
}
