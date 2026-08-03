using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Payments
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly PaymentGatewaySettings _paymentGatewaySettings;
        private readonly PaymentIntentService _paymentIntentService = new(); // class from package stripe
        
        public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
        {
            _paymentGatewaySettings = options.Value;
            StripeConfiguration.ApiKey = options.Value.SecretKey;//
        }

        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            var options = new PaymentIntentCreateOptions()
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes =  ["card"]

            };

            var intent = await _paymentIntentService.CreateAsync(options, cancellationToken:ct);
            return new PaymentIntentResult(intent.Id , intent.ClientSecret);
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default)
        {
            var options = new PaymentIntentUpdateOptions()
            {
                Amount = (long) amount
            };
            var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, options, cancellationToken: ct);
            return new PaymentIntentResult(intent.Id,intent.ClientSecret);
        }
    }
}
