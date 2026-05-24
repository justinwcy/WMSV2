using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.DbContexts;

namespace FacilityService.DbContexts
{
    public class FacilityDbContext(DbContextOptions<FacilityDbContext> options) :
        BaseDbContext(options)
    {
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseStaff> WarehouseStaffs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FacilityDbContext).Assembly);
        }
    }
}
