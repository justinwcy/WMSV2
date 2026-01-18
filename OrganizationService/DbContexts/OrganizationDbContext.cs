using MassTransit;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.AddTransactionalOutboxEntities();
        }
    }
}
