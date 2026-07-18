using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Specification;
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

        public async Task<int> Countasync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec);
            return await query.CountAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default) =>
            await _dbContext.Set<TEntity>().ToListAsync(ct);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default)
        {
            #region bad way
            ////1) Entery Point
            //IQueryable<TEntity> query = _dbContext.Set<TEntity>();
            ////2) where xx

            ////3) include 
            //if (spec is not null)
            //{
            //    if (spec.Includeexpressions.Any())
            //    {
            //        foreach (var expression in spec.Includeexpressions)
            //        {
            //            query = query.Include(expression);
            //        }
            //    }
            //}

            ////sort

            ////paganation
            ////instead of this do evaluator *************************************************
            #endregion
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec);
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default) =>
            await _dbContext.Set<TEntity>().FindAsync(id,ct);

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), spec);
            return await query.FirstOrDefaultAsync(ct);
        }

        public void Remove(TEntity entity) =>
            _dbContext.Set<TEntity>().Remove(entity);



        public void Update(TEntity entity) =>
            _dbContext.Set<TEntity>().Update(entity);
        
    }
}
