using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> repositories = [];
        
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typename = typeof(TEntity).Name;
            if (repositories.TryGetValue(typename,out object? value))
            {
                return (IGenericRepository<TEntity,TKey>)value;
            }
            else
            {
                var repo = new GenericRepository<TEntity, TKey>(dbContext);
                repositories[typename] = repo;
                return repo;
            }
        }
         
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await dbContext.SaveChangesAsync(ct);
        }
    }
}
