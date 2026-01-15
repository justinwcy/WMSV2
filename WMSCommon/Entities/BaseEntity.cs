namespace WMSCommon.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid CompanyId { get; set; }
    }
}
