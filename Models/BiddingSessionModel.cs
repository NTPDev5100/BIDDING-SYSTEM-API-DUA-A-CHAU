using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CoreContants;

namespace Models
{
    public class BiddingSessionModel : DomainModels.AppDomainModel
    {

        /// <summary>
        /// id gói thầu
        /// </summary>
        public Guid? BiddingId { get; set; }
        /// <summary>
        /// Mã người dùng
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên phiên đấu thầu
        /// </summary>
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
        public int? Status { get; set; }


        /// <summary>
        /// Tên trạng thái
        /// </summary>
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case (int)StatusBiddingSession.ChuaDienRa:
                        return "Chưa diễn ra";
                    case (int)StatusBiddingSession.DangDienRa:
                        return "Đang diễn ra";
                    case (int)StatusBiddingSession.DaKetThuc:
                        return "Đã kết thúc";
                    case (int)StatusBiddingSession.DaDong:
                        return "Đã đóng";
                    default:
                        return string.Empty;
                }
            }
        }


        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Hình ảnh gói thầu
        /// </summary>
        public string Thumbnail { get; set; }



        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// Thời gian còn lại
        /// </summary>
        public double? BiddingSessionTimeOut { get; set; }


        /// <summary>
        /// Cờ tham gia bỏ thầu
        /// </summary>
        public bool? IsBid { get; set; }

        /// <summary>
        /// Id sản phẩm của gói thầu
        /// </summary>
        public Guid? ProductId { get; set; }


        /// <summary>
        /// Tên người tạo
        /// </summary>
        public string CreatedName { get; set; }

        /// <summary>
        /// Id job schedule trên hangfire
        /// </summary>
        public string JobHangFireId { get; set; }
    }
}
