using Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.CoreContants;

namespace Models
{
    public class RoleModel : AppDomainCatalogueModel
    {
        /// <summary>
        /// Danh sách chức năng được truy cập
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>
        /// Danh sách menu được truy cập
        /// </summary>
        public string MenuList { get; set; }
        /// <summary>
        /// Cấp số role
        /// </summary>
        public int? RoleNumberLevel { get; set; }

    }
}
