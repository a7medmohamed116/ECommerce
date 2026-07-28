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
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto ,CancellationToken ct = default)
        {
            var order = await _orderService.CreateOrderAsync(orderDto, GetUserEmail()!, ct);
            return ToActionResult(order);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken ct = default)
            => ToActionResult(await _orderService.GetOrdersForSpecificUser(GetUserEmail(),ct));

        [Authorize]
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid Id, CancellationToken ct = default)
            => ToActionResult(await _orderService.GetOrderByIdAndEmailUser(Id, GetUserEmail(),ct));

        [AllowAnonymous] // anyone can reach this 
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken ct)
            => ToActionResult(await _orderService.GetDeliveyMethods(ct));

        
        


        
    }
}
