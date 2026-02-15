using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class IncomingConfiguration : IEntityTypeConfiguration<Incoming>
    {
        public void Configure(EntityTypeBuilder<Incoming> builder)
        {
            builder.HasKey(i => i.Id);
            builder.HasMany(i => i.IncomingDetails)
                .WithOne(d => d.Incoming)
                .HasForeignKey(d => d.IncomingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Vendor)
                .WithMany(v => v.Incomings)
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.PONumber,
            });
        }
    }
}
