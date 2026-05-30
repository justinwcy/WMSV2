using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalesService.Models;

namespace SalesService.Configurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.ProductDetails)
                .WithOne(d => d.Shop)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Name,
            });
        }
    }
}