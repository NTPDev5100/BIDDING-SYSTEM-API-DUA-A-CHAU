using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;
using static Utilities.CoreContants;

namespace Models
{
    public class BiddingTicketModel : DomainModels.AppDomainModel
    {


        /// <summary>
        /// Id Phiên đấu thầu
        /// </summary>
        public Guid? BiddingSessionId { get; set; }

        /// <summary>
        /// Id Gói thầu của phiên tham gia
        /// </summary>
        public Guid? BiddingId { get; set; }

        /// <summary>
        /// Số lượng sản phẩm
        /// </summary>
        public int? Quantity { get; set; }
        public string QuantityFormat
        {
            get
            {
                try
                {
                    return String.Format("{0:0,0}", Quantity);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Định giá
        /// </summary>
        public decimal? Price { get; set; }

        public string PriceFormat
        {
            get
            {
                try
                {
                    return String.Format("{0:0,0 vnđ}", Price);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }

        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case (int)StatucBiddingTicket.ChoDuyet:
                        return "Chờ duyệt";
                    case (int)StatucBiddingTicket.ChoKetQua:
                        return "Chờ kết quả";
                    case (int)StatucBiddingTicket.TrungThau:
                        return "Trúng thầu";
                    case (int)StatucBiddingTicket.RotThau:
                        return "Rớt thầu";
                    case (int)StatucBiddingTicket.Huy:
                        return "Hủy";
                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tên gói thầu
        /// </summary>
        public string BiddingName { get; set; }
        /// <summary>
        /// Tên phiên thầu
        /// </summary>
        public string BiddingSessionName { get; set; }

        /// <summary>
        /// Hình sản phẩm
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// Danh sách chuỗi nhân viên phụ trách
        /// </summary>
        public string PersonInCharge { get; set; }
        /// <summary>
        /// Danh sách nhân viên phụ trách
        /// </summary>
        public List<ObjectJsonCustom> PersonInChargeNameList
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<ObjectJsonCustom>>(PersonInCharge.ToString());
                }
                catch { return null; }
            }
        }

        public string DateCreate
        {
            get
            {
                try
                {
                    return Timestamp.UnixTimestampToDateTime(Created.Value).ToString("dd-MM-yyyy HH:mm:ss");
                }
                catch { return null; }
            }
        }


    }
}
