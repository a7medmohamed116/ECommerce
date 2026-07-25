using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    //owend for orderitem
    public class ProductItemOrdered //snapshot in the moment we create order copy data as was in selling time  // if after month updated the main product the history will not modify the old orders cause i took a copy int the past
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;

    }
}
