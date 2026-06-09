using FulfilmentService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.DbContexts;

namespace FulfilmentService.DbContexts
{
    public class FulfilmentDbContext(DbContextOptions<FulfilmentDbContext> options) :
        BaseDbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FulfilmentDbContext).Assembly);
        }
    }
}
