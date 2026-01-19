using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Models;

namespace OrganizationService.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.Id);

            builder
                .HasMany(c => c.Staffs)
                .WithOne(s => s.StaffCompany)
                .HasForeignKey(s => s.StaffCompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
