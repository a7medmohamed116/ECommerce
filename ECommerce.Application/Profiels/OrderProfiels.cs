using AutoMapper;
using ECommerce.Application.DTOs.IdentityDTOs;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiels
{
    public class OrderProfiels : Profile
    {
        public OrderProfiels()
        {
            //CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.DeliveryMethodCost, opt => opt.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderPictureUrlResolver>());
                //images / ../product1.png XXXX
                //should be  : https://localhost:7200/images by resolver
        }
    }
}
