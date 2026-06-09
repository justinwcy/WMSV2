using FulfilmentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FulfilmentService.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(pd => pd.Id);
            builder.OwnsOne(p => p.ProductDimensions);
            builder.HasMany(pd => pd.OrderDetails)
                .WithOne(od => od.ProductDetail)
                .HasForeignKey(od => od.ProductDetailId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasIndex(pd => new
            {
                pd.CompanyId,
                pd.Name,
            });
        }
    }
}
