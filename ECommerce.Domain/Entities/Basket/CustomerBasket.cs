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


    }
}
