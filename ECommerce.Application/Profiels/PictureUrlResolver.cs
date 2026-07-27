using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiels
{
    public class PictureUrlResolver : IValueResolver<Product, ProductDto, string>//automapper
    {
        private readonly UrlSettings _urlSettings;

        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSettings = options.Value;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            //source => images/products/formalBlazor.jpg
            //reurn => https://localhost:7270/files/images/products/formalBlazor.jpg
             var baseurl = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseurl}/Files/{path}";
        }
        //images / ../product1.png XXXX
        //should be  : https://localhost:7200/images by resolver
    }

    public class UrlSettings
    {
        public string BaseUrl { get; set; } = default!;
    }
}
