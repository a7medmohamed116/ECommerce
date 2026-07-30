using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId ,CancellationToken ct = default)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntent(basketId, ct);
            return ToActionResult(result);

        }   
    }
}
