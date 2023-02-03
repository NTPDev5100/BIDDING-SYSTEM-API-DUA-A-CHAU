using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Repository
{
    public interface ICatalogueRepository<T> : IDomainRepository<T> where T : AppDomainCatalogue
    {
        T GetByCode(string code);
    }
}
