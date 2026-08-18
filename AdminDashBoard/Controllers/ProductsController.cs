using AdminDashBoard.Models.Products;
using AdminDashBoard.Services;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace AdminDashBoard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductApiClient _productApiClient;

        public ProductsController(IUnitOfWork unitOfWork ,ProductApiClient productApiClient)
        {
            _unitOfWork = unitOfWork;
            _productApiClient = productApiClient;
        }
        public async Task<IActionResult> Index()
        {
            var productRepo =  _unitOfWork.GetRepository<Product, int>();
            var queryParames = new ProductQueryParams();
            var spec = new ProductWithBrandAndTypeSpec(queryParames,true);
            var products = await productRepo.GetAllAsync(spec);
            var productModel = products.Select(P => new ProductViewModel
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                Price = P.Price,
                PictureUrl = $"https://localhost:7270/Files/{P.PictureUrl.TrimStart('/')}",
                BrandId = P.BrandId,
                TypeId = P.TypeId,
                Brand = P.productBrand,
                Type = P.productType
            });          
            return View(productModel);
        }

        [HttpGet]
        public async Task<IActionResult> Createa()
        {
            var brands = await _productApiClient.GetBrandsAsync();
            var types = await _productApiClient.GetTypesAsync();

            ViewBag.Brands = new SelectList(brands, "Id", "Name");
            ViewBag.Types = new SelectList(types, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Createa(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _productApiClient.CreateAsync(model);
            if (result is null)
            {
                Console.WriteLine("errrrrrrrrrrrrrrrrrrrrrrror");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productApiClient.GetByIdAsync(id);
            if (product is null)
                return NotFound();
            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                BrandId = product.BrandId,
                TypeId = product.TypeId
            };

            var brands = await _productApiClient.GetBrandsAsync();
            var types = await _productApiClient.GetTypesAsync();

            ViewBag.Brands = new SelectList(brands, "Id", "Name", model.BrandId);
            ViewBag.Types = new SelectList(types, "Id", "Name", model.TypeId);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updateDto = new UpdateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                TypeId = model.TypeId,
                Picture = model.Image
            };

            await _productApiClient.UpdateAsync(model.Id, updateDto);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletea(int id)
        {
            var result = await _productApiClient.DeleteAsync(id);

            if (!result)
            {
                TempData["Error"] = "Failed to delete product.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
