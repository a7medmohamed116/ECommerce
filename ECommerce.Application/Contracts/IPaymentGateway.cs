using ECommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        //Create payment intent
        // Amount , currency => PaymentIntentId + Client Secret
        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default);

        //Update Payment Intent
        // paymentintentid (old) + Amount => new paymentintentid + clientsecret
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken ct = default);
    }
}
