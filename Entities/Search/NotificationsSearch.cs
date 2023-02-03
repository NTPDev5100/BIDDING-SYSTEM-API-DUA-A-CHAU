using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class NotificationsSearch: DomainEntities.BaseSearch
    {

        /// <summary>
        /// Lọc theo userId (Người nhận thông báo)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Đã xem
        /// </summary>
        public bool? IsSeen { get; set; }

    }
}
