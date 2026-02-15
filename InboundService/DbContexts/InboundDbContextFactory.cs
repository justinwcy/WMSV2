using Microsoft.EntityFrameworkCore;

namespace CatalogService.DbContexts
{
    public class InboundDbContextFactory(string connectionString) : IDbContextFactory<InboundDbContext>
    {
        public InboundDbContext CreateDbContext()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<InboundDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new InboundDbContext(dbContextOptionBuilder);
        }
    }
}
