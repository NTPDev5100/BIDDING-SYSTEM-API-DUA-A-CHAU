using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.DomainModels
{
    public class AppDomainFileModel : AppDomainModel
    {
        /// <summary>
        /// Tên file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Loại file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Đuôi file
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tên lưu trong thư mục
        /// </summary>
        public string FileRandomName { get; set; }

        /// <summary>
        /// Link download File
        /// </summary>
        public string FileUrl { get; set; }

    }
}
