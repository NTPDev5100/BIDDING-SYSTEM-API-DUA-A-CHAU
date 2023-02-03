using Entities.DomainEntities;
using Interface.DbContext;
using Interface.Repository;
using Interface.UnitOfWork;
using Service.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class AppUnitOfWork : UnitOfWork, IAppUnitOfWork
    {
        readonly IAppDbContext appDbContext;
        public AppUnitOfWork(IAppDbContext context) : base(context)
        {
            appDbContext = context;
        }
        public override ICatalogueRepository<T> CatalogueRepository<T>()
        {
            return new CatalogueRepository<T>(appDbContext);
        }
        public override IDomainRepository<T> Repository<T>()
        {
            return new AppRepository<T>(appDbContext);
        }
    }
}
