using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Basket
{
    public class CustomerBasket // in memory (no sql) data base
    {
        public string Id { get; set; } =default!; // created guid from side [frontend]
        public ICollection<BasketItem> Items { get; set; } = [];

       //payment
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }

    }
}
