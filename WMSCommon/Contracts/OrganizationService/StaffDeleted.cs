namespace WMSCommon.Contracts.OrganizationService
{
    public class StaffDeleted<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}