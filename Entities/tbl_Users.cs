using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Entities
{
    public class tbl_Users : DomainEntities.DomainEntities
    {
        /// <summary>
        /// Mã người dùng
        /// </summary>
        [Description("Mã người dùng")]
        public string Code { get; set; }

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
        /// Ngày sinh
        /// </summary>
        [Description("Ngày sinh")]
        public double? Birthday { get; set; }
        /// <summary>
        /// Chứng minh nhân dân
        /// </summary>
        [Description("Chứng minh nhân dân")]
        [StringLength(50)]
        public string IdentityCard { get; set; }

        /// <summary>
        /// Ngày cấp chứng minh nhân dân
        /// </summary>
        [Description("Ngày cấp chứng minh nhân dân")]
        public double? IdentityCardDate { get; set; }

        /// <summary>
        /// Nơi cấp chứng minh nhân dân
        /// </summary>
        [Description("Nơi cấp chứng minh nhân dân")]
        [StringLength(1000, ErrorMessage = "Tên phải nhỏ hơn 300 kí tự")]
        public string IdentityCardAddress { get; set; }

        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        [StringLength(4000)]
        public string Password { get; set; }

        /// <summary>
        /// Giới tính
        /// 0 => Khác
        /// 1 => Nam
        /// 2 => Nữ
        /// </summary>
        [Description("Giới tính")]
        public int? Gender { get; set; }
        /// <summary>
        /// Cờ cho biết đây là admin
        /// </summary>
        public bool? IsAdmin { get; set; }
        /// <summary>
        /// Danh sách chức vụ của người dùng
        /// </summary>
        [Description("Danh sách chức vụ của người dùng")]
        public string Roles { get; set; }

        public string Thumbnail { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [Description("Ghi chú")]
        public string Note { get; set; }
        public int? TimeZone { get; set; }
        public string OneSignal_DeviceId { get; set; }
    }

    
}
