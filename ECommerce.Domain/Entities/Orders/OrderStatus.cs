using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        Pending =1,
        processing,
        Shipped,
        Delivered,
        Cancelled,
        PaymentFailed,
        PaymentSuceeded

    }
}
