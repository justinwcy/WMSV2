using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityService.Configurations
{
    public class RackConfiguration : IEntityTypeConfiguration<Rack>
    {
        public void Configure(EntityTypeBuilder<Rack> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.Warehouse)
                .WithMany(w => w.Racks)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Name,
            });
        }
    }
}