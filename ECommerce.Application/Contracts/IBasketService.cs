using ECommerce.Application.Common;
using ECommerce.Application.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IBasketService
    {
        Task<Result<BasketDto>>GetBasketAsync(string basketid , CancellationToken ct =default);
        Task<Result<BasketDto>> CreateOrUpdateAsync(BasketDto basket, TimeSpan? TTL = default, CancellationToken ct = default);
        Task<Result> DeleteBasketAsync(string basketId ,CancellationToken ct =default);
    }
}
