using Entities.DomainEntities;
using Entities.Project;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Project
{
    public interface IProjectUserService : IDomainService<ProjectUsers, BaseSearch>
    {
        //Task<string> UpdateStatusTaskOfUser(int UserId, int TaskId);
    }
}
