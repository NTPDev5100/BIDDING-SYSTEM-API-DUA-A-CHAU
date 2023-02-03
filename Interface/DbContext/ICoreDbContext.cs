using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Interface.DbContext
{
    public interface ICoreDbContext : IAppDbContext
    {
        //DbSet<TEntity> Set<TEntity>() where TEntity : class;
        //DbQuery<TQuery> Query<TQuery>() where TQuery : class;
        //EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        //int SaveChanges();
        //ChangeTracker ChangeTracker { get; }
        //int SaveChanges(bool acceptAllChangesOnSuccess);
        //Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        //Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        //DatabaseFacade Database { get; }
    }
}
