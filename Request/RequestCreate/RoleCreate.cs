using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;
using Request.DomainRequests;
using Utilities;
using static Utilities.CoreContants;

namespace Request.RequestCreate
{
    public class RoleCreate : RequestCatalogueCreateModel
    {
        /// <summary>
        /// Cấp số role
        /// </summary>
        [Required(ErrorMessage ="Yêu cầu chọn quyền!")]
        public int RoleNumberLevel { get; set; }
    }
}
