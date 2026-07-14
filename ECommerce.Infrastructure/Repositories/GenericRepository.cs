using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(TEntity entity)=>
        
             _dbContext.Set<TEntity>().Add(entity);


        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) =>
            await _dbContext.Set<TEntity>().ToListAsync(ct);



        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default) =>
            await _dbContext.Set<TEntity>().FindAsync(id,ct);
        

        public void Remove(TEntity entity) =>
            _dbContext.Set<TEntity>().Remove(entity);



        public void Update(TEntity entity) =>
            _dbContext.Set<TEntity>().Update(entity);
        
    }
}
