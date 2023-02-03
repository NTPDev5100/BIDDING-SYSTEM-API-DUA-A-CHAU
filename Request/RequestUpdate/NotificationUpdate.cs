using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class NotificationUpdate : DomainRequests.DomainUpdate
    {
        /// <summary>
        /// Đã xem
        /// </summary>
        public bool? IsSeen { get; set; }

    }
}
