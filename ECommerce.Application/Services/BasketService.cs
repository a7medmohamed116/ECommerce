using AutoMapper;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.BasketDTOs;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepo ,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        public async Task<Result<BasketDto>> CreateOrUpdateAsync(BasketDto basket, TimeSpan? TTL = null, CancellationToken ct = default)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket); 
            var basketResult = await _basketRepo.CreateOrUpdateBasketAsync(customerBasket, TTL, ct);
            return basketResult == null ? Result<BasketDto>.Fail(Error.Failure("Failed to create or update basket")) : Result<BasketDto>.OK(_mapper.Map<BasketDto>(basketResult));
        }

        public async Task<Result> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _basketRepo.DeleteBasketAsync(basketId, ct);
            return result ? Result.OK() : Result.Fail(Error.Failure("Failed to delete basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string basketid, CancellationToken ct = default)
        {
            var basket = await _basketRepo.GetBasketAsync(basketid, ct);
            return basket == null ? Result<BasketDto>.Fail(Error.Failure("Failed To Get Basket")) : Result<BasketDto>.OK(_mapper.Map<BasketDto>(basket));
        }
    }
}
