using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class OrederSpecification :BaseSpecification<Order,Guid>
    {
        public OrederSpecification(string email):base (O=>O.BuyerEmail == email)  //getall order
        {
            //include deliverymethods and items
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrederByDesc(X => X.OrderDate);
        }
        public OrederSpecification(Guid id,string email) : base(O => O.BuyerEmail == email && O.Id == id)  //getall order
        {
            //include deliverymethods and items
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrederByDesc(X => X.OrderDate);
        }
    }
}
