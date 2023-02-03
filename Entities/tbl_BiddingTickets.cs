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
    public class tbl_BiddingTickets : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Id Phiên đấu thầu
        /// </summary>
        [Required]
        [Description("Id của phiên tham gia")]
        public Guid? BiddingSessionId { get; set; }

        /// <summary>
        /// Id Gói thầu của phiên tham gia
        /// </summary>
        [Description("Id Gói thầu của phiên tham gia")]
        public Guid? BiddingId { get; set; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        [Required]
        public int? Quantity { get; set; }


        /// <summary>
        /// Định giá
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [Description("Trạng thái")]
        public int? Status { get; set; }


        /// <summary>
        /// Tên công ty
        /// </summary>
        [NotMapped]
        public string CompanyName { get; set; }


        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        [NotMapped]
        public string FullName { get; set; }

        /// <summary>
        /// Tên gói thầu
        /// </summary>
        [NotMapped]
        public string BiddingName { get; set; }
        /// <summary>
        /// Tên phiên thầu
        /// </summary>
        [NotMapped]
        public string BiddingSessionName { get; set; }

        /// <summary>
        /// Hình sản phẩm
        /// </summary>
        [NotMapped]
        public string Thumbnail { get; set; }


        /// <summary>
        /// Danh sách nhân viên phụ trách
        /// </summary>
        [NotMapped]
        public string PersonInCharge { get; set; }


    }
}
