using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.OrderDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class OrderController :ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost("Order")]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto ,CancellationToken ct = default)
        {
            var order = await _orderService.CreateOrderAsync(orderDto, GetUserEmail()!, ct);
            return ToActionResult(order);
        }
    }
}
