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
    public interface IProjectServiceService : IDomainService<ProjectServices, SearchProject>
    {
        ///// <summary>
        ///// Cập nhật dịch vụ gần hết hạn
        ///// </summary>
        ///// <returns></returns>
        //Task UpdateServiceExprireDate();
        //Task<List<ProjectServices>> Mona_sp_Load_ProjectService_EndDate();
    }
}
