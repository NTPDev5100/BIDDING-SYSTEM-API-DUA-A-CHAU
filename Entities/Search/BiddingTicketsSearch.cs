using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class BiddingTicketsSearch : BaseSearch
    {
        /// <summary>
        /// Từ ngày
        /// </summary>
        public double? StartDate { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        public double? EndDate { get; set; }

        /// <summary>
        /// Số tiền bắt đầu
        /// </summary>
        public string StartPrice { get; set; }
        /// <summary>
        /// Số tiền kết thúc
        /// </summary>
        public string EndPrice { get; set; }

        /// <summary>
        /// Số lượng bắt đầu
        /// </summary>
        public string StartQuantity { get; set; }
        /// <summary>
        /// Số lượng kết thúc
        /// </summary>
        public string EndQuantity { get; set; }



        /// <summary>
        /// Lọc theo id phiên đấu thầu
        /// </summary>
        public string BiddingSessionId { get; set; }

        /// <summary>
        /// Lọc theo id gói thầu
        /// </summary>
        public Guid? BiddingId { get; set; }

        

        /// <summary>
        /// Lọc theo trạng thái
        /// </summary>
        public string Status { get; set; }


        /// <summary>
        /// Lọc theo người tạo ra phiếu
        /// </summary>
        public Guid? CreatedBy { get; set; }

        public string role { get; set; }
        

    }
}
