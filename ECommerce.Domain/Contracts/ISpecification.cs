using ECommerce.Domain.Entities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ISpecification<TEntity,TKey> where TEntity:BaseEntity<TKey>
    {
        //Includes
        ICollection<Expression<Func<TEntity,object>>> Includeexpressions { get; }

        Expression<Func<TEntity,bool>> Criteria { get; }

        Expression<Func<TEntity,object>> OrderBy { get; }
        Expression<Func<TEntity,object>> OrderByDescending { get; }
        int Skip { get; }
        int Take { get; }
        bool ISPaginated { get; }


    }
    //contract will use it in app so implement in app (basespec) => will need include so do overload in igeneric repo in infra
    // will update in generic repo  and to reduce code do spec folder with helper spec evaluator  in infra and good 
    // then edit in service in productservice change getallproducts(ct) will direct deal with ispec or basespec?
    // no we need a specific spec for product only  in app we will do productwithbrandandtypespec 


     
    // ispec => base => evaluator => igeneric => generic => edit service with the specific spec class
}
