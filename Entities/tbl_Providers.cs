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
    public class tbl_Providers : DomainEntities.DomainEntities
    {
        /// <summary>
        /// UserName
        /// </summary>
        [Required]
        [StringLength(50)]
        [Description("Tên đăng nhập")]
        public string Username { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [Description("Tên đầy đủ")]
        [StringLength(500)]
        public string FullName { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        [Description("Tên công ty")]
        [StringLength(500)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Description("Số điện thoại")]
        [StringLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Description("Email")]
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [Description("Địa chỉ")]
        [StringLength(1000)]
        public string Address { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [Description("Trạng thái")]
        public int? Status { get; set; }

        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        [StringLength(4000)]
        public string Password { get; set; }

        /// <summary>
        /// Danh sách chức vụ của người dùng
        /// </summary>
        [Description("Danh sách chức vụ của người dùng")]
        public string Roles { get; set; }

        public string Thumbnail { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        [StringLength(50)]
        [Description("Mã số thuế")]
        public string TaxCode { get; set; }

        /// <summary>
        /// Nhân viên phụ trách
        /// </summary>
        [Description("Nhân viên phụ trách")]
        public string PersonInCharge { get; set; }

        /// <summary>
        /// Tên Người tạo
        /// </summary>
        [NotMapped]
        public string CreatedName { get; set; }


        public string OneSignal_DeviceId { get; set; }

    }
}
