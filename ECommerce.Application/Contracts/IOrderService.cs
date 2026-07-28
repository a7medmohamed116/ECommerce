using ECommerce.Application.Common;
using ECommerce.Application.DTOs.OrderDTOs;
using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveyMethods(CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetOrdersForSpecificUser(string email, CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailUser(Guid OrderId ,string email, CancellationToken ct = default);
    }
}
