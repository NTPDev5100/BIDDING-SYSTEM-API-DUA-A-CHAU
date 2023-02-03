using AutoMapper;
using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Interface.DbContext;
using Interface.Services;
using Interface.UnitOfWork;
using Service.Services.DomainServices;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BiddingService : DomainService<tbl_Biddings, BiddingsSearch>, IBiddingService
    {
        protected IAppDbContext coreDbContext;
        public BiddingService(IAppUnitOfWork unitOfWork, IMapper mapper, IAppDbContext coreDbContext) : base(unitOfWork, mapper)
        {
            this.coreDbContext = coreDbContext;
        }

        protected override string GetStoreProcName()
        {
            return "Bidding_GetPagingData";
        }

        public async Task<int> GetCountBiddingByProductId(Guid ProductId)
        {
            var count = await Task.FromResult(Queryable.Where(x => x.ProductId == ProductId && x.Deleted == false).AsNoTracking().Count());
            return count;
        }

    }
}
