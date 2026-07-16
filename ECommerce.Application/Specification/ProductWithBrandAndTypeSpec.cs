using ECommerce.Application.Common;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Specification
{
    public class ProductWithBrandAndTypeSpec :BaseSpecification<Product, int>
    {
        //get all
        public ProductWithBrandAndTypeSpec(ProductQueryParams queryParams) : base
            (p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId) && 
            (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId) &&
            (string.IsNullOrWhiteSpace(queryParams.Search) ||p.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
            AddInclude(P => P.productBrand);
            AddInclude(P => P.productType);

            switch (queryParams.Sort)
            {
                case ProductSortOptions.NameAscending:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortOptions.NameDescending:
                    AddOrederByDesc(P => P.Name);
                    break;
                case ProductSortOptions.PriceAscending:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortOptions.PriceDescending:
                    AddOrederByDesc(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize , queryParams.PageIndex);

            

        } // 
        // get product by id
        public ProductWithBrandAndTypeSpec(int id):base(p=>p.Id == id)
        {
            AddInclude(P => P.productBrand);
            AddInclude(P => P.productType);

        }
    }
}
