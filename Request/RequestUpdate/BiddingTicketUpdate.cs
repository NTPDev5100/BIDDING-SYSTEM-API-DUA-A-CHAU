using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class BiddingTicketUpdate : DomainRequests.DomainUpdate
    {   
        /// <summary>
        /// Trạng thái kết quả của phiếu
        /// </summary>
        public int? Status { get; set; }
    }
}
