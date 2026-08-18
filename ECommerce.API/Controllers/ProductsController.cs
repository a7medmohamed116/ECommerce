using ECommerce.API.Attributes;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Authorization;
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
        [RedisCache(90)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>>GetAllProducts( [FromQuery] ProductQueryParams queryParams ,CancellationToken ct = default)//int? brandId ,int? typeId the pramters too match with serarch , oreder and paganation so go to queryparams
        {
            var result = await _productService.GetAllProductsAsync(queryParams, ct);//|| above in controller we pass the data as object ProductQueryParams queryParams so will deal with it as body and no body in get  request so error must say [FromQuery]
            return ToActionResult(result);
        }
        // baseurl/api/products/{id}
        [Authorize( Roles="SuperAdmin")]
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

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create ([FromForm] CreateProductDto model)
        {
            var result = await _productService.CreateAsync(model);

            return ToActionResult(result);
        }


    }
}
