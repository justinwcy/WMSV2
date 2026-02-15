using InboundService.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class InboundOrderDetailConfiguration : IEntityTypeConfiguration<InboundOrderDetail>
    {
        public void Configure(EntityTypeBuilder<InboundOrderDetail> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(d => d.Incoming)
                .WithMany(i => i.IncomingDetails)
                .HasForeignKey(d => d.IncomingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(d => d.Status).HasConversion<string>();
            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Status,
            });
        }
    }
}
