using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasKey(v => v.Id);
            builder.HasMany(v => v.InboundOrders)
                .WithOne(i => i.Vendor)
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Name,
            });
        }
    }
}
