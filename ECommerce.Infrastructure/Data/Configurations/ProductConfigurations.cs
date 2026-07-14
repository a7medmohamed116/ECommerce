using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(P => P.productBrand)
                   .WithMany()
                   .HasForeignKey(P => P.BrandId);
            builder.HasOne(P => P.productType)
                   .WithMany()
                   .HasForeignKey(P => P.TypeId);
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");
            builder.Property(P => P.Name).HasMaxLength(100);
            builder.Property(P => P.Description).HasMaxLength(500);
            builder.Property(P => P.PictureUrl).HasMaxLength(200);

        }
    }
}
