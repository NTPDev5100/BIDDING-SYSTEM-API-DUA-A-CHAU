using AutoMapper;
using Entities.Catalogue;
using Entities.Search;
using Interface.Services.Catalogue;
using Interface.UnitOfWork;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Catalogue
{
    public class TechnicalOptionService : CatalogueService<tbl_TechnicalOptions, TechnicalOptionsSearch>, ITechnicalOptionService
    {
        public TechnicalOptionService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            IsUseStore = false;
        }

    }
}
