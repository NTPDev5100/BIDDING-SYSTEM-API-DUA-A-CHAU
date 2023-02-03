using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.Services.Catalogue;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IProductsService : ICatalogueService<tbl_Products, ProductSearch>
    {
    }
}
