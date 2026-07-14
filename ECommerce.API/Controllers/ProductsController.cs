using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController] // no need them anymore
    public class ProductsController : ApiBaseController // edit not to inherit from  ControllerBase become  ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //Get baseurl/api/products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>>GetAllProducts(CancellationToken ct = default)
        {
            var result = await _productService.GetAllProductsAsync(ct);
            return ToActionResult(result);
        }
        // baseurl/api/products/{id}
        [HttpGet ("{id}")] 
        public async Task<ActionResult<ProductDto>>GetProduct(int id ,CancellationToken ct = default)
        {
            var result = await _productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }

        //Get baseurl/api/products/brands


        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>>GetAllBrands(CancellationToken ct)
        {
            var result = await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }

        //Get baseurl/api/products/types

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        {
            var result = await _productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }


    }
}
