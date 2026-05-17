using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InboundService.DbContexts;

public class InboundDesignTimeDbContextFactory 
    : IDesignTimeDbContextFactory<InboundDbContext>
{
    public InboundDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<InboundDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=InboundDb");

        return new InboundDbContext(optionsBuilder.Options);
    }
}