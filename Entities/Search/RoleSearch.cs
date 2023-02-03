using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Search
{
    public class RoleSearch : BaseSearch
    {
        /// <summary>
        /// Lọc role theo mã code
        /// </summary>
        public string Code { get; set; }
    }
}
