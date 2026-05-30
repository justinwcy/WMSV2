using Microsoft.EntityFrameworkCore;
using SalesService.Models;
using WMSCommon.DbContexts;

namespace SalesService.DbContexts
{
    public class SalesDbContext(DbContextOptions<SalesDbContext> options) :
        BaseDbContext(options)
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
        }
    }
}
