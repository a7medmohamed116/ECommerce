using ECommerce.Application.Common;
using ECommerce.Application.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface IPaymentService
    {
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntent(string basketId , CancellationToken ct =default );
        Task PaymentSucceeded(string paymentIntentId);
        Task PaymentFailed(string paymentIntentId);

    }
}
