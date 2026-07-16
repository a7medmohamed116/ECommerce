using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entrypoint , ISpecification<TEntity,TKey> spec ) where TEntity : BaseEntity<TKey>
        {
            //1) entry point
            var query = entrypoint;
            //2) where

            if (spec.Criteria is not null)
            {
                query =  query.Where(spec.Criteria);
            }

            //3)include
            #region withfor each
            //if (spec.Includeexpressions.Any())
            //{
            //    foreach (var expression in spec.Includeexpressions)
            //    {
            //        query = query.Include(expression);
            //    }
            //} == 
            #endregion
            query = spec.Includeexpressions.Aggregate(query, (current, nextexp) => current.Include(nextexp));

            //4) OrderNy
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if
            (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            //5) Pagination
            if (spec.ISPaginated)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            } 


            return query;

        }

    }
}
