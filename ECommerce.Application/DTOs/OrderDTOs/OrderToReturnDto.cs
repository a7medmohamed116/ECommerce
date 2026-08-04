using ECommerce.Application.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDto ShipToAddress { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public decimal Total { get; set; }  //Mapped Automatic From Order Cause itis name GetTotal by convintion

        public string? PaymentIntentId { get; private set; }
        public DateTimeOffset? PaidAtUtc { get; private set; }
    }
}
