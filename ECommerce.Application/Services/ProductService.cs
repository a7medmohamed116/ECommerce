using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Specification;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public ProductService(IUnitOfWork unitOfWork ,IMapper mapper , IImageService imageService )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<Result<ProductDto>> CreateAsync(CreateProductDto model)
        {
            var brand = await _unitOfWork
                .GetRepository<ProductBrand, int>()
                .GetByIdAsync(model.BrandId);

            if (brand is null)
                return Result<ProductDto>.Fail(Error.NotFound ("Brand not found"));

            var type = await _unitOfWork
                .GetRepository<ProductType, int>()
                .GetByIdAsync(model.TypeId);

            if (type is null)
                return Result<ProductDto>.Fail(Error.NotFound("Type not found"));

            if (model.Image is null)
                return Result<ProductDto>.Fail(Error.Validation("Product image is required"));

            var pictureUrl = await _imageService.SaveImageAsync(
                model.Image,
                "products");

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                BrandId = model.BrandId,
                TypeId = model.TypeId,
                PictureUrl = pictureUrl
            };

            _unitOfWork
                .GetRepository<Product, int>()
                .Add(product);

            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);

            return Result<ProductDto>.OK(productDto);
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var product = await _unitOfWork
                .GetRepository<Product,int>()
                .GetByIdAsync(id);

            if (product == null)
                return Result<bool>.Fail(Error.NotFound(
                    "Product not found"));

            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                _imageService.DeleteImageAsync(product.PictureUrl);
            }

            _unitOfWork.GetRepository<Product,int>().Remove(product);

            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.OK(true);
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            var mapped = _mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.OK(mapped);
        }

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default) // instead of return ireadonlylist we will retuen paginatedresult so can show response with index , size ,count,data
        {
            var spec = new ProductWithBrandAndTypeSpec(queryParams);  
            var products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec ,ct);
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);
            var countspec = new ProductCountSpecification(queryParams);
            var countofallspecproducts = await _unitOfWork.GetRepository<Product,int>().Countasync(countspec, ct);
            var result =  new PaginatedResult<ProductDto>(queryParams.PageIndex,queryParams.PageSize, countofallspecproducts, data);//products.Count [i already konw i got  4 products in pagesize 4 etc]is wrong cause i need count all products in specific certirea (بيدج سابز 4 تمام لكن الكاونت الي محقق الكونديشن المعين  هيكون 13 مثلا لو بجيت كل البرودكتس) //do new method in generic repo to count with new spec class productcountspec
            return Result<PaginatedResult<ProductDto>>.OK(result);
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var data = _mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.OK(data);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec ,ct);
            if (product is null) return Result<ProductDto>.Fail(Error.NotFound("product not found", $"product with id {id} not found"));
            var data = _mapper.Map<ProductDto>(product);
            return Result<ProductDto>.OK(data);
        }

        public async Task<Result<ProductDto>> UpdateAsync(int id,UpdateProductDto model)
        {
            var product = await _unitOfWork.GetRepository<Product,int>()
                .GetByIdAsync(id);

            if (product == null)
                return Result<ProductDto>.Fail(Error.NotFound(
                    "Product not found"));

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.BrandId = model.BrandId;
            product.TypeId = model.TypeId;

            if (model.Picture != null)
            {
                if (!string.IsNullOrEmpty(product.PictureUrl))
                {
                    _imageService.DeleteImageAsync(product.PictureUrl);
                }

                product.PictureUrl =
                    await _imageService.SaveImageAsync(model.Picture, "products");
            }

            _unitOfWork.GetRepository<Product,int>().Update(product);

            await _unitOfWork.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);

            return Result<ProductDto>.OK(productDto);
        }
    }
}
