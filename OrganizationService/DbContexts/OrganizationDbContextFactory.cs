using Microsoft.EntityFrameworkCore;
using OrganizationService.DbContexts;

namespace UserService.DbContexts
{
    public class OrganizationDbContextFactory(string connectionString) : IDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<OrganizationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new OrganizationDbContext(dbContextOptionBuilder);
        }
    }
}
