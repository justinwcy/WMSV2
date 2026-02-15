using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class InboundOrderConfiguration : IEntityTypeConfiguration<InboundOrder>
    {
        public void Configure(EntityTypeBuilder<InboundOrder> builder)
        {
            builder.HasKey(i => i.Id);
            builder.HasMany(i => i.IncomingDetails)
                .WithOne(d => d.Incoming)
                .HasForeignKey(d => d.IncomingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Vendor)
                .WithMany(v => v.InboundOrders)
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(i => i.Source).HasConversion<string>();

            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.PONumber,
            });
        }
    }
}
