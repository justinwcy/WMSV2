using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OrganizationService.Models;

namespace OrganizationService.Configurations
{
    public class StaffRoleConfiguration : IEntityTypeConfiguration<StaffRole>
    {
        public void Configure(EntityTypeBuilder<StaffRole> builder)
        {
            builder.HasOne(r => r.Role)
                .WithMany()
                .HasForeignKey(r => r.RoleId)
                .IsRequired();

            builder.HasOne(r => r.Staff)
                .WithMany(s => s.UserRoles)
                .HasForeignKey(r => r.UserId)
                .IsRequired();
        }
    }
}
