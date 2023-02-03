using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class BiddingSessionsSearch : BaseSearch
    {
        /// <summary>
        /// Lọc theo tên phiên đấu thầu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lọc theo gói thầu
        /// </summary>
        public Guid? BiddingId { get; set; }
        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// Lọc theo nhà cung cấp
        /// </summary>

        public Guid? ProviderId { get; set; }
    }
}
