using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class ProviderSearch : BaseSearch
    {
        /// <summary>
        /// Lọc theo tài khoản
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Lọc theo tên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Lọc theo tên công ty
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Lọc theo cờ active
        /// </summary>
        public bool? Active { get; set; }
    }
}
