using AutoMapper;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiels
{
    internal class OrderPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings  _urlSettings;

        public OrderPictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSettings = options.Value;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var url = _urlSettings.BaseUrl.TrimEnd('/');
            var path = source.Product.PictureUrl.TrimStart('/');
            return $"{url}/Files/{path}"; 
        }
    }
}
