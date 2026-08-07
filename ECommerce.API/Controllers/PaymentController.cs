using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECommerce.API.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly PaymentGatewaySettings _payment;

        public PaymentController(IPaymentService paymentService,IOptions<PaymentGatewaySettings> options)
        {
            _paymentService = paymentService;
            _payment = options.Value;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId ,CancellationToken ct = default)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntent(basketId, ct);
            return ToActionResult(result);

        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            //read request with open reader stream
            var requestJson = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                //check with eventUtility
                //stripe signature and WhSecret
                var stripeEvent = EventUtility.ConstructEvent(requestJson, Request.Headers["Stripe-Signature"], _payment.WebhookSecret);

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded :
                        var paymentIntentSucc = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntentSucc is not null)
                            await _paymentService.PaymentSucceeded(paymentIntentSucc.Id);
                        break;
                    case EventTypes.PaymentIntentPaymentFailed :
                        var paymentIntentFail = stripeEvent.Data.Object as PaymentIntent;
                        if (paymentIntentFail is not null)
                            await _paymentService.PaymentFailed(paymentIntentFail.Id);
                        break;
                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message); 
            }
            
        }

    }//when get type take the payment intent id and make the payment service change the ordre status
}
