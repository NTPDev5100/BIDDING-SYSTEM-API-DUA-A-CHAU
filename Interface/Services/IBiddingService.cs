using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface IBiddingService : IDomainService<tbl_Biddings, BiddingsSearch>
    {
        Task<int> GetCountBiddingByProductId(Guid ProductId);
    }
}
