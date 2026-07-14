using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiels
{
    public class ProductProfiels : Profile
    {
        public ProductProfiels()
        {
            CreateMap<Product, ProductDto>()
                                          .ForMember(dest => dest.productBrand, opt => opt.MapFrom(src => src.productBrand.Name))
                                          .ForMember(dest => dest.productType, opt => opt.MapFrom(src => src.productType.Name));
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType,TypeDto>();
            
        }


        

    }
}
