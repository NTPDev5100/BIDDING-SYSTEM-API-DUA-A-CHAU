using System;
using System.Collections.Generic;
using System.Text;

namespace Request.Auth
{
    public class Role
    {
        /// <summary>
        /// Tên chức năng (menu)
        /// </summary>
        public string roleName { get; set; }

        /// <summary>
        /// Quyền của chức năng
        /// </summary>
        public bool isView { get; set; }
    }
}
