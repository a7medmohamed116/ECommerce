using AutoMapper;
using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiels
{
    public class BasketProfiels : Profile
    {
        public BasketProfiels()
        {
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        }
    }
}
