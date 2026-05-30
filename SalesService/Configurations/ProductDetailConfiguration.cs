using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesService.Models;

namespace SalesService.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(pd => pd.Id);
            builder.HasOne(pd => pd.Shop)
                .WithMany(s => s.ProductDetails)
                .HasForeignKey(pd => pd.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pd => new
            {
                pd.CompanyId,
                pd.Name,
            });
        }
    }
}