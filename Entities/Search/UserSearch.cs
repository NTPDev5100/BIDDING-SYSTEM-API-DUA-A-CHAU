using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static Utilities.CatalogueEnums;

namespace Entities.Search
{
    public class UserSearch : BaseSearch
    {
        /// <summary>
        /// Lọc theo trạng thái: 0.Chưa kích hoạt 1.Kích hoạt 2.Khóa
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// Lọc theo chức vụ
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Lọc theo tài khoản
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Lọc theo tên
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Lọc theo số điện thoại
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Lọc theo giới tính
        /// </summary>
        public int? Gender { get; set; }
    }
}
