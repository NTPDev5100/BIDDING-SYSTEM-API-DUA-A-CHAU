using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NotificationModel : DomainModels.AppDomainModel
    {
        /// <summary>
        /// Id người nhận thông báo
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// Đã xem
        /// </summary>
        public bool? IsSeen { get; set; }

        /// <summary>
        /// Đường link thông báo
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public int? IsType { get; set; }
    }
}
