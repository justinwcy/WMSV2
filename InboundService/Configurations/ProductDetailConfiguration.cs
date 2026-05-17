using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(c => c.Id);
            builder.OwnsOne(p => p.ProductDimensions);

            builder.HasIndex(s => new
            {
                s.CompanyId,
            });
        }
    }
}
