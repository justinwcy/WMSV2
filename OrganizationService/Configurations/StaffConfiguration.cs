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
                .HasOne(s => s.StaffCompany)
                .WithMany(c => c.Staffs)
                .HasForeignKey(s => s.StaffCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => new
            {
                CompanyId = s.StaffCompanyId,
                s.UserName,
                s.Email,
                s.FirstName,
                s.LastName
            });
        }
    }
}
