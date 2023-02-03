using AutoMapper;
using Entities.Catalogue;
using Entities.DomainEntities;
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
    public class MenuService : CatalogueService<tbl_Menu, BaseSearch>, IMenuService
    {
        public MenuService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
