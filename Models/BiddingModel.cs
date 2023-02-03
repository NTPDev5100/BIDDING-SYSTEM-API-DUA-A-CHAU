using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BiddingModel : DomainModels.AppDomainModel
    {
        /// <summary>
        /// Mã gói thầu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên gói thầu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả gói thầu
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Id của sản phẩm
        /// </summary>
        public Guid? ProductId { get; set; }
        /// <summary>
        /// Tên Sản phẩm
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Tên người tạo
        /// </summary>
        public string CreatedName { get; set; }

    }
}
