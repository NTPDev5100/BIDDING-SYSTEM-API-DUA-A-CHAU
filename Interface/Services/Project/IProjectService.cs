using Entities.Project;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Project
{
    public interface IProjectService : IDomainService<Projects, SearchProject>
    {
        /// <summary>
        /// Job tính số ngày dự án chạy định kỳ 1 ngày
        /// </summary>
        /// <returns></returns>
        //Task JobDayLeftProject();
    }
}
