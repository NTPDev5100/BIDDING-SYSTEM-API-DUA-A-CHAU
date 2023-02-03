using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestCreate
{
    public class BiddingSessionCreate : DomainRequests.DomainCreate
    {
        /// <summary>
        /// Tên phiên đấu thầu
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập tên phiên!")]
        [MaxLength(500, ErrorMessage = "Tên phiên nhập tối đa 500 ký tự!")]
        public string Name { get; set; }
        /// <summary>
        /// id gói thầu
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn gói thầu của phiên")]
        public Guid? BiddingId { get; set; }
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
        [Range(1, Int32.MaxValue, ErrorMessage = "Vui lòng nhập số lượng tối thiểu lớn hơn 0!")]
        public int? MinimumQuantity { get; set; }
        /// <summary>
        /// Số lượng tối đa
        /// </summary>
        [Range(1, Int32.MaxValue, ErrorMessage ="Vui lòng nhập số lượng tối đa lớn hơn 0!")]
        public int? MaximumQuantity { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Mô tả nhập tối đa 1000 ký tự!")]
        public string Description { get; set; }

    }
}
