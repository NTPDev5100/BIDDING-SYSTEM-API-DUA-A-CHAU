using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Request.DomainRequests;
using Utilities;


namespace Request.RequestUpdate
{
    public class RoleUpdate : RequestCatalogueUpdateModel
    {
        /// <summary>
        /// Danh sách chức năng
        /// </summary>
        public List<Permission> Permissions { get; set; }
        /// <summary>
        /// Danh sách menu được truy cập (Định dạng Json)
        /// </summary>
        public string MenuList { get; set; }
        /// <summary>
        /// Cấp số role
        /// </summary>
        [Required(ErrorMessage = "Yêu cầu chọn quyền!")]
        public int? RoleNumberLevel { get; set; }
    }
}
