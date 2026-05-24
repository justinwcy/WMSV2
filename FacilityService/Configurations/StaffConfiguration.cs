using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityService.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.Warehouses)
                .WithMany(w => w.Staffs)
                .UsingEntity<WarehouseStaff>(
                    j => j.HasOne(ws => ws.Warehouse)
                        .WithMany()
                        .HasForeignKey(ws => ws.WarehouseId)
                        .OnDelete(DeleteBehavior.Cascade),
                    
                    j => j.HasOne(ws => ws.Staff)
                        .WithMany()
                        .HasForeignKey(ws => ws.StaffId)
                        .OnDelete(DeleteBehavior.Cascade),
                    
                    j =>
                    {
                        j.HasKey(ws => ws.Id); 
                        j.HasIndex(ws => new { ws.WarehouseId, ws.StaffId }).IsUnique();
                    });
            
            builder.HasMany(s => s.Racks)
                .WithMany(w => w.Staffs)
                .UsingEntity<RackStaff>(
                    j => j.HasOne(rs => rs.Rack)
                        .WithMany()
                        .HasForeignKey(rs => rs.RackId)
                        .OnDelete(DeleteBehavior.Cascade),
                    
                    j => j.HasOne(rs => rs.Staff)
                        .WithMany()
                        .HasForeignKey(rs => rs.StaffId)
                        .OnDelete(DeleteBehavior.Cascade),
                    
                    j =>
                    {
                        j.HasKey(rs => rs.Id); 
                        j.HasIndex(rs => new { rs.RackId, rs.StaffId }).IsUnique();
                    });

            
            builder.HasIndex(s => new
            {
                s.CompanyId,
                s.Email,
            });
        }
    }
}