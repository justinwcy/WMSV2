namespace CatalogService.Models
{
    public class AuditEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string EntityName { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string PrimaryKey { get; set; }
        public string OldValues { get; set; } // JSON
        public string NewValues { get; set; } // JSON
        public string AffectedColumns { get; set; } // JSON list of changed properties
    }
}
