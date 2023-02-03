using Entities;
using Entities.Search;
using Request.RequestCreate;
using Request.RequestUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface ITechnicalProductService: DomainServices.IDomainService<tbl_TechnicalProduct, TechnicalProductSearch>
    {
        Task AddItemTechnicalProduct(TechnicalProductCreate createModel, Guid idUserCreated);
        Task UpdateItemTechnicalProduct(TechnicalProductUpdate updateModel, Guid idUserUpdate);
        Task DeleteTechnicalByProductId(Guid productId);
        Task<int> GetCountTechnicalByOptionId(Guid technicalOptionId);
    }
}
