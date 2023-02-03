using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_BiddingSessions : DomainEntities.DomainEntities
    {

        /// <summary>
        /// id gói thầu
        /// </summary>
        [Description("Id gói thầu của phiên")]
        public Guid? BiddingId { get; set; }

        /// <summary>
        /// Mã phiên đấu thầu
        /// </summary>
        [StringLength(100)]
        [Description("Mã phiên đấu thầu")]
        public string Code { get; set; }
        /// <summary>
        /// Tên phiên đấu thầu
        /// </summary>
        [Description("Tên phiên đấu thầu")]
        [StringLength(500)]
        public string Name { get; set; }
        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public double? StartDate { get; set; }
        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public double? EndDate { get; set; }
        /// <summary>
        /// Số lượng tối thiểu
        /// </summary>
        public int? MinimumQuantity { get; set; }
        /// <summary>
        /// Số lượng tối đa
        /// </summary>
        public int? MaximumQuantity { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [Description("Trạng thái")]
        public int? Status { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(1000)]
        [Description("Mô tả")]
        public string Description { get; set; }


        /// <summary>
        /// Id job schedule trên hangfire
        /// </summary>
        public string JobHangFireId { get; set; }

        [NotMapped]
        public string Thumbnail { get; set; }

        [NotMapped]
        public string ProductName { get; set; }

        [NotMapped]
        public double? BiddingSessionTimeOut { get; set; }

        [NotMapped]
        public Guid? ProductId { get; set; }

        [NotMapped]
        public string CreatedName { get; set; }
    }
}
