using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(X => X.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(X => X.Product, product =>
            {
                product.Property(P => P.ProductName).HasMaxLength(100);
                product.Property(P => P.PictureUrl).HasMaxLength(200);
            });
        }
    }
}
