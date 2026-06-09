using FulfilmentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FulfilmentService.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.Id);
            builder.HasOne(od => od.ProductDetail)
                .WithMany(pd => pd.OrderDetails)
                .HasForeignKey(od => od.ProductDetailId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(od => new
            {
                od.CompanyId,
                od.OrderId,
            });
        }
    }
}
