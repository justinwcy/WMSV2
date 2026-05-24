using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityService.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(w => w.Id);
            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Name,
            });
        }
    }
}