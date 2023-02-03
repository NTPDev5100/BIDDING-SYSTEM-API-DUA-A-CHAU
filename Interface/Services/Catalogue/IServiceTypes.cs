using Entities.Catalogue;
using Entities.DomainEntities;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services.Catalogue
{
    public interface IServiceTypes : ICatalogueService<ServiceTypes, BaseSearch>
    {
        Task<bool> CheckServiceTypeExistProjectService(Guid id);
    }
}
