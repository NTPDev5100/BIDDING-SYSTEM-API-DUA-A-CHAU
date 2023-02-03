using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Request.DomainRequests;
using Utilities;
using static Utilities.CatalogueEnums;

namespace Request.RequestCreate
{
    public class UserCreate : DomainCreate
    {

        [MaxLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự!")]
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        public string Username { set; get; }

        [StringLength(12, ErrorMessage = "Số kí tự của số điện thoại phải lớn hơn 8 và nhỏ hơn 12!", MinimumLength = 9)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]+${9,11}", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessage = "Số kí tự của email phải nhỏ hơn 50!")]
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email có định dạng không hợp lệ!")]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(1000, ErrorMessage = "Số kí tự của email phải nhỏ hơn 1000!")]
        public string Address { get; set; }

        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        [Required(ErrorMessage = "Vui lòng nhập Password!")]
        [StringLength(255, ErrorMessage = "Mật khẩu phải lớn hơn 8 kí tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        //[JsonIgnore]
        [StringLength(200, ErrorMessage = "Họ tên phải nhỏ hơn 200 kí tự")]
        public string FullName
        {
            //get
            //{
            //    return FirstName + " " + LastName;
            //}
            get;set;
        }
        
        ///// <summary>
        ///// Ngày sinh
        ///// </summary>
        //public double? Birthday { get; set; }

        ///// <summary>
        ///// Chứng minh nhân dân
        ///// </summary>
        //[StringLength(50, ErrorMessage = "Số ký tự phải nhỏ hơn 50")]
        //public string IdentityCard { get; set; }

        /// <summary>
        /// Giới tính
        /// 0 => Khác
        /// 1 => Nam
        /// 2 => Nữ
        /// </summary>
        public int? Gender { get; set; }
        public string Thumbnail { get; set; }
        ///// <summary>
        ///// Ghi chú
        ///// </summary>
        //public string Note { get; set; }
        /// <summary>
        /// Danh sách chức vụ của người dùng
        /// </summary>
        [Required(ErrorMessage = "Vui lòng chọn chức vụ!")]
        public string Roles { get; set; }
    }
}
