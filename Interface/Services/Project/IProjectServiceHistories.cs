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
    public interface IProjectServiceHistories : IDomainService<ProjectServiceHistories, SearchProject>
    {
        /// <summary>
        /// Get item ProjectServiceHistories
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<ProjectServiceHistories> DetailProjectServiceHistories(Guid Id);

        /// <summary>
        /// List Project Serivice Histories By ProjectServiceID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<List<ProjectServiceHistories>> ListProjectServiceHistories(Guid ProjectService);


    }
}
