using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class TechnicalProductSearch : DomainEntities.BaseSearch
    {
        /// <summary>
        /// Lọc theo sản phẩm
        /// </summary>
        public Guid? ProductId { get; set; }

    }
}
