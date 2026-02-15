using Microsoft.EntityFrameworkCore;

namespace CatalogService.DbContexts
{
    public class CatalogDbContextFactory(string connectionString) : IDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new CatalogDbContext(dbContextOptionBuilder);
        }
    }
}
