using InboundService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.DbContexts;

namespace CatalogService.DbContexts
{
    public class InboundDbContext(DbContextOptions<InboundDbContext> options) :
        BaseDbContext(options)
    {
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<InboundOrder> InboundOrders { get; set; }
        public DbSet<InboundOrderDetail> InboundOrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InboundDbContext).Assembly);
        }
    }
}
