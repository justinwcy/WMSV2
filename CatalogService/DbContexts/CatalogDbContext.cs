using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.DbContexts;

namespace CatalogService.DbContexts
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) :
        BaseDbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }

    }
}
