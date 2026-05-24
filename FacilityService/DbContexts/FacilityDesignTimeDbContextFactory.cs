using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FacilityService.DbContexts;

public class FacilityDesignTimeDbContextFactory 
    : IDesignTimeDbContextFactory<FacilityDbContext>
{
    public FacilityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FacilityDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=FacilityDb");

        return new FacilityDbContext(optionsBuilder.Options);
    }
}