using Microsoft.EntityFrameworkCore;
using WMSCommon.Models;

using MassTransit;
using Wolverine.EntityFrameworkCore;

namespace WMSCommon.DbContexts
{
    public class BaseDbContext(DbContextOptions options) :
        DbContext(options)
    {
        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.MapWolverineEnvelopeStorage(); 
        }
    }
}
