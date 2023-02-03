using Entities.DomainEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using static Utilities.CatalogueEnums;

namespace Interface.Services.DomainServices
{
    public interface ICatalogueService<T, E> : IDomainService<T, E> where T : AppDomainCatalogue where E : BaseSearch
    {
        T GetByCode(string code);
        Task<AppDomainImportResult> ImportTemplateFile(Stream stream, string CreatedBy);
    }
}
