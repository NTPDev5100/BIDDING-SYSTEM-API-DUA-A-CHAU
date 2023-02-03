using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Request.DomainRequests
{
    public class AppDomainFileRequestl : DomainCreate
    {
        /// <summary>
        /// Tên file
        /// </summary>
        [StringLength(500, ErrorMessage = "Tên file không được dài quá 500 kí tự")]
        public string fileName { get; set; }

        /// <summary>
        /// Loại file
        /// </summary>
        [StringLength(100, ErrorMessage = "Tên loại file không được dài quá 100 kí tự")]
        public string contentType { get; set; }

        /// <summary>
        /// Đuôi file
        /// </summary>
        [StringLength(50, ErrorMessage = "Tên đuôi file không được dài quá 50 kí tự")]
        public string fileExtension { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(1000, ErrorMessage = "Mô tả không được dài quá 1000 kí tự")]
        public string description { get; set; }

        /// <summary>
        /// Tên lưu trong thư mục
        /// </summary>
        public string fileRandomName { get; set; }

        /// <summary>
        /// Link download File
        /// </summary>
        public string fileUrl { get; set; }
    }
}
