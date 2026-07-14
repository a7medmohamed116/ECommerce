using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Products
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public ProductBrand productBrand { get; set; }
        public int BrandId { get; set; }
        public ProductType  productType { get; set; }
        public int TypeId { get; set; }

    }
}
