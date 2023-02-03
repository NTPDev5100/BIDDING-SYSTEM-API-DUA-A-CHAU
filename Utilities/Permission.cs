using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Permission
    {
        /// <summary>
        /// Mã chức năng cha
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Tên chức năng cha
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Danh sách chức năng con
        /// </summary>
        public List<PermissionActions> PermissionActions { get; set; }
    }
    public class PermissionActions
    {
        /// <summary>
        /// Mã chức năng con
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Tên chức năng con
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Quyền được truy cập chức năng
        /// </summary>
        public bool Allowed { get; set; }
    }
}
