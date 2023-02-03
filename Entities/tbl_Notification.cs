using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class tbl_Notification : DomainEntities.DomainEntities
    {

        /// <summary>
        /// Id người nhận thông báo
        /// </summary>
        [Description("Id người nhận thông báo")]
        public Guid? UserId { get; set; }
        /// <summary>
        /// Tiêu đề thông báo
        /// </summary>
        [Description("Tiêu đề thông báo")]
        public string Title { get; set; }
        /// <summary>
        /// Nội dung thông báo
        /// </summary>
        [Description("Nội dung thông báo")]
        public string Content { get; set; }
        /// <summary>
        /// Đã xem
        /// </summary>
        [Description("Đã xem")]
        public bool? IsSeen { get; set; }


        /// <summary>
        /// Đường link thông báo
        /// </summary>
        [Description("Đường link của thông báo")]
        public string Url { get; set; }

        /// <summary>
        /// Loại thông báo
        /// </summary>
        [Description("Loại thông báo")]
        public int? IsType { get; set; }

    }
    //public class Get_Notification : DomainEntities.DomainEntities
    //{
    //    public int? UserId { get; set; }
    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //    /// <summary>
    //    /// Đã xem
    //    /// </summary>
    //    public bool? IsSeen { get; set; }
    //    public int TotalRow { get; set; }
    //}
}
