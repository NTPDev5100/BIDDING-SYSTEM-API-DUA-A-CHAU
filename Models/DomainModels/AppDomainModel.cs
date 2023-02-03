using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models.DomainModels
{
    public class AppDomainModel
    {

        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public double? Created { get; set; }

        /// <summary>
        /// Tạo bởi
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public double? Updated { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Cờ active
        /// </summary>
        [DefaultValue(true)]
        public bool? Active { get; set; }
    }
}
