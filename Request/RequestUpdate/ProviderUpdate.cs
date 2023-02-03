using Request.DomainRequests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.RequestUpdate
{
    public class ProviderUpdate : DomainUpdate
    {
        /// <summary>
        /// UserName
        /// </summary>
        [MaxLength(128, ErrorMessage = "Tên đăng nhập tối đa 128 ký tự!")]
        //[Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập!")]
        public string Username { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [StringLength(200, ErrorMessage = "Tên nhà cung cấp phải nhỏ hơn 200 kí tự!")]
        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp!")]
        public string FullName { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        [StringLength(200, ErrorMessage = "Tên công ty phải nhỏ hơn 200 kí tự!")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(12, ErrorMessage = "Số kí tự của số điện thoại phải lớn hơn 8 và nhỏ hơn 12!", MinimumLength = 9)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]+${9,11}", ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string Phone { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(50, ErrorMessage = "Số kí tự của email phải nhỏ hơn 50!")]
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [StringLength(1000, ErrorMessage = "Số kí tự của email phải nhỏ hơn 1000!")]
        public string Address { get; set; }


        /// <summary>
        /// Mật khẩu người dùng
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Xác nhận mật khẩu
        /// </summary>
        //[StringLength(128, ErrorMessage = "Mật khẩu xác nhận phải có ít nhất 8 kí tự và tối đa 128 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không giống với mật khẩu!")]
        public string ConfirmPassword { get; set; }

        public string Thumbnail { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Nhân viên phụ trách
        /// </summary>
        public string PersonInCharge { get; set; }
        /// <summary>
        /// Cờ active
        /// </summary>
        public bool? Active { get; set; }
    }

}
