using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class PaymentSpecification : BaseSpecification<Order,Guid>
    {
        public PaymentSpecification(string paymentIntentId):base(P=>P.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
