using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.Services;
using Interface.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Catalogue
{
    public class ProductService : DomainServices.CatalogueService<tbl_Products, ProductSearch>, IProductsService
    {
        public ProductService(IAppUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            IsUseStore = false;
        }

        
    }
}
