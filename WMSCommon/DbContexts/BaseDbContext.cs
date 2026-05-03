using Microsoft.EntityFrameworkCore;
using WMSCommon.Models;

namespace WMSCommon.DbContexts
{
    public class BaseDbContext(DbContextOptions options) :
        DbContext(options)
    {
        public DbSet<AuditEntry> AuditEntries { get; set; }
    }
}
