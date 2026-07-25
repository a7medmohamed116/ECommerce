using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Orders.Order>
    {

        public void Configure(EntityTypeBuilder<Domain.Entities.Orders.Order> builder)
        {
            builder.HasMany(X => X.Items)
                   .WithOne();
            builder.Property(X => X.SubTotal).HasColumnType("decimal(8,2)");
            builder.OwnsOne(X => X.ShipToAddress,address =>
            {
                address.Property(ad => ad.FirstName).HasMaxLength(50);
                address.Property(ad => ad.LastName).HasMaxLength(50);
                address.Property(ad => ad.City).HasMaxLength(50);
                address.Property(ad => ad.Street).HasMaxLength(50);
                address.Property(ad => ad.Country).HasMaxLength(50);
            });

            builder.Property(X => X.Status).HasConversion<String>().HasMaxLength(50);//
            
        }
    }
}
