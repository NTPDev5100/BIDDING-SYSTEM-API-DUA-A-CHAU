using Entities.Project;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interface.Services.Project
{
    public interface IProjectCommentService : IDomainService<ProjectComments, SearchProjectComment>
    {
        /// <summary>
        /// Danh sách ghi chú của dự án
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        Task<List<ProjectComments>> ListCommentOfProject(Guid ProjectId);
    }
}
