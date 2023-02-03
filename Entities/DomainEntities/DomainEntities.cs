using Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.DomainEntities
{
    public class DomainEntities
    {
        public DomainEntities()
        {
            //Created = DateTime.Now;
        }

        /// <summary>
        /// Tổng số item phân trang
        /// </summary>
        [NotMapped]
        public int TotalItem { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID dữ liệu")]
        public Guid Id { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Description("Thời gian tạo")]
        public double? Created { get; set; }

        /// <summary>
        /// Tạo bởi
        /// </summary>
        [Description("Người tạo")]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [Description("Thời gian cập nhật")]
        public double? Updated { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        [Description("Người cập nhật")]
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Cờ xóa
        /// </summary>
        public bool? Deleted { get; set; } = false;

        /// <summary>
        /// Cờ active
        /// </summary>
        public bool? Active { get; set; } = true;

    }
}
