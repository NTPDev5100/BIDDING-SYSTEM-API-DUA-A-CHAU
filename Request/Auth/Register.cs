using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Request.Auth
{
    public class Register
    {
        /// <summary>
        /// Email hoặc Số điện thoại
        /// </summary>
        [MaxLength(128, ErrorMessage = "Tên đăng nhập tối đa 128 ký tự")]
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc nhập")]
        public string Username { set; get; }

        [StringLength(128, ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự và tối đa 128 ký tự", MinimumLength = 8)]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc nhập")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [StringLength(128, ErrorMessage = "Mật khẩu xác nhận phải có ít nhất 8 kí tự và tối đa 128 ký tự", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không giống với mật khẩu")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [StringLength(50, ErrorMessage = "Số kí tự của email phải nhỏ hơn 50!")]
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email có định dạng không hợp lệ!")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(12, ErrorMessage = "Số kí tự của số điện thoại phải lớn hơn 8 và nhỏ hơn 12!", MinimumLength = 9)]
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]+${9,11}", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }
    }
}
