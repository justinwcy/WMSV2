using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InboundService.Configurations
{
    public class IncomingDetailConfiguration : IEntityTypeConfiguration<IncomingDetail>
    {
        public void Configure(EntityTypeBuilder<IncomingDetail> builder)
        {
            builder.HasKey(i => i.Id);

            builder.HasOne(d => d.Incoming)
                .WithMany(i => i.IncomingDetails)
                .HasForeignKey(d => d.IncomingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Status,
            });
        }
    }
}
