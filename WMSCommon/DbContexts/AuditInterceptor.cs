using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WMSCommon.Contexts;
using WMSCommon.Models;

namespace WMSCommon.DbContexts
{
    public class AuditInterceptor(IUserContext userContext) : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges(eventData.Context);
            if (auditEntries.Any())
            {
                eventData.Context.Set<AuditEntry>().AddRange(auditEntries);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private List<AuditEntry> OnBeforeSaveChanges(DbContext context)
        {
            context.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditEntry || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry
                {
                    EntityName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    Timestamp = DateTime.UtcNow,
                    UserId = userContext.UserId
                };

                var oldValues = new Dictionary<string, object>();
                var newValues = new Dictionary<string, object>();
                var changedColumns = new List<string>();

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        // If it's a new record, ensure the ID is generated now so we can audit it
                        if (property.Metadata.ClrType == typeof(Guid) &&
                            (Guid)property.CurrentValue == Guid.Empty)
                        {
                            property.CurrentValue = Guid.NewGuid();
                        }
                        auditEntry.PrimaryKey = property.CurrentValue?.ToString();
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            oldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                changedColumns.Add(propertyName);
                                oldValues[propertyName] = property.OriginalValue;
                                newValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                auditEntry.OldValues = JsonSerializer.Serialize(oldValues);
                auditEntry.NewValues = JsonSerializer.Serialize(newValues);
                auditEntry.AffectedColumns = JsonSerializer.Serialize(changedColumns);

                auditEntries.Add(auditEntry);
            }

            return auditEntries;
        }
    }
}
