using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProviderModel : DomainModels.AppDomainModel
    {
        /// <summary>
        /// UserName
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string CompanyName { get; set; }

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
        /// Mật khẩu người dùng
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Danh sách chức vụ của người dùng
        /// </summary>
        public string Roles { get; set; }

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
        /// Tên Người tạo
        /// </summary>
        public string CreatedName { get; set; }

    }
}
