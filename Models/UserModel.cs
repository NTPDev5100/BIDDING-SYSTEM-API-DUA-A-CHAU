using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Models
{ 

    public class UserModel : AppDomainModel
    {
        /// <summary>
        /// Mã người dùng
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Họ
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public double? Birthday { get; set; }

        /// <summary>
        /// Chứng minh nhân dân
        /// </summary>
        public string IdentityCard { get; set; }

        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Giới tính
        /// 0 => Khác
        /// 1 => Nam
        /// 2 => Nữ
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// Danh sách chức vụ của người dùng
        /// </summary>
        public string Roles { get; set; }
        public string Thumbnail { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; }
        public string OneSignal_DeviceId { get; set; }
    }

    
}
