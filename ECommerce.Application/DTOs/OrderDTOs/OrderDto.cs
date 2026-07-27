using ECommerce.Application.DTOs.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.OrderDTOs
{
    public class OrderDto // input with useremail to create address 
    {
        public int DeliveryMethodId { get; set; }
        public string BasketId { get; set; } = default!;
        public AddressDto ShipToAddress { get; set; } = default!; //will mapped to orderaddress which owned by order
    }
}
