using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Models;

namespace OrganizationService.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasMany(s => s.UserTokens)
                .WithOne(ut => ut.Staff)
                .HasForeignKey(ut => ut.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(s => s.Company)
                .WithMany(c => c.Staffs)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new
            {
                CompanyId = s.CompanyId,
                s.UserName,
                s.Email,
                s.FirstName,
                s.LastName
            });
        }
    }
}
