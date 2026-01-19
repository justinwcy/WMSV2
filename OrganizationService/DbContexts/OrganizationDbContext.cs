using MassTransit;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using OrganizationService.Constants;
using OrganizationService.Models;

namespace OrganizationService.DbContexts
{
    public class OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) :
        IdentityDbContext<Staff, IdentityRole<Guid>, Guid>(options)
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationDbContext).Assembly);

            SeedRoles(modelBuilder);
            SeedCompany(modelBuilder);

            modelBuilder.AddTransactionalOutboxEntities();
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            string[] allRoles = Enum.GetNames<Role>();

            int counter = 0;
            List<IdentityRole<Guid>> identityRoles = new List<IdentityRole<Guid>>();
            foreach (string roleName in allRoles)
            {
                string guidString = $"00000000-0000-0000-0000-{counter:D12}";
                identityRoles.Add(new IdentityRole<Guid>
                {
                    Id = new Guid(guidString),
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant()
                });
            }
            
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(identityRoles);
        }

        private void SeedCompany(ModelBuilder modelBuilder)
        {
            Company initialCompany = new Company()
            {
                Id = new Guid("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
                Name = "AETech Technology and Services Enterprise",
                Staffs = new List<Staff>(),
            };
            modelBuilder.Entity<Company>().HasData(initialCompany);
        }
    }
}
