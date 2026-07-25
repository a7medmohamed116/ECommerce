using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    public class Order :BaseEntity<Guid>
    {
        public Order(string buyerEmail, OrderAddress shipToAddress, ICollection<OrderItem> items, decimal subTotal, DeliveryMethod deliveryMethod )
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            Items = items;
            SubTotal = subTotal;
            DeliveryMethod = deliveryMethod;
        }

        private Order() // Empty parameterless ctor for EF Core
        {
            
        }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string BuyerEmail { get; set; } = default!;
        public OrderAddress ShipToAddress { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];

        public decimal SubTotal { get; set; } //Price Of Product * Quantity
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }//FK
        public decimal GetTotal() => SubTotal + (DeliveryMethod?.Price ?? 0);  //method not mapped to data base SubTotal + DeliveryMethod Cost

        
    }
}
