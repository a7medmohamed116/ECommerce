using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> Includeexpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            Includeexpressions.Add(include);
        }
        //protected void AddCondition(Expression<Func<TEntity, bool>> condtion)
        //{
        //    Criteria = condtion;
        //} // force who use base entity to send cond
        public BaseSpecification(Expression<Func<TEntity, bool>> condtion) //base
        {
            Criteria = condtion;
        }

        public Expression<Func<TEntity, object>> OrderBy {  get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending {  get; private set; }

        

        protected void AddOrderBy(Expression<Func<TEntity, object>> OrderByexp)
        {
            OrderBy = OrderByexp;   

        }
        protected void AddOrederByDesc(Expression<Func<TEntity, object>> OrderByDescExp)
        {
            OrderByDescending = OrderByDescExp;
        }

        public int Skip {get;private set;} 
                
        public int Take { get; private set; }

        public bool ISPaginated { get;private set;}

        protected void ApplyPagination(int pagesize , int pageindex)
        {
            ISPaginated = true;
            Take = pagesize;
            //etc20/30  3   -1  2 *10 = 20 so skip first 20       
            Skip = (pageindex-1)*pagesize;

        }
        // etc 40 => 10 10 10 10
    }
}
