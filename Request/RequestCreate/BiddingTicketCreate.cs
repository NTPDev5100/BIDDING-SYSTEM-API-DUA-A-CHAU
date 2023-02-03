using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class BiddingTicketCreate : DomainRequests.DomainCreate
    {
        /// <summary>
        /// Phiên đấu thầu
        /// </summary>
        [Required]
        public Guid? BiddingSessionId { get; set; }


        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        [Required(ErrorMessage ="Vui lòng nhập số lượng!")]
        [Range(1,10000000, ErrorMessage ="Số lượng lớn hơn 1 và nhỏ hơn 100000")]
        public int? Quantity { get; set; }


        /// <summary>
        /// Định giá
        /// </summary>
        [Required(ErrorMessage ="Vui lòng nhập giá tiền!")]
        public decimal? Price { get; set; }
    }
}
