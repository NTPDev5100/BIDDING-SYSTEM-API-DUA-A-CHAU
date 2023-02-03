using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class BiddingsSearch : BaseSearch
    {
        /// <summary>
        /// Lọc theo tên phiên đấu thầu
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Lọc theo sản phẩm
        /// </summary>
        public Guid? ProductId { get; set; }
    }
}
