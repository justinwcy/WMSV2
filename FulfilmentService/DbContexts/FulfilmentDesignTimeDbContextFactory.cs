using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FulfilmentService.DbContexts;

public class FulfilmentDesignTimeDbContextFactory 
    : IDesignTimeDbContextFactory<FulfilmentDbContext>
{
    public FulfilmentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FulfilmentDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=FulfilmentDb");

        return new FulfilmentDbContext(optionsBuilder.Options);
    }
}