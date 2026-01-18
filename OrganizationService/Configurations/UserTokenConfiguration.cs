using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Models;

namespace OrganizationService.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(ua => ua.Id);

            builder
                .HasOne(ut => ut.Staff)
                .WithMany(s => s.UserTokens)
                .HasForeignKey(ut => ut.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => new
            {
                s.CompanyId, UserId = s.StaffId,
            });
        }
    }
}
