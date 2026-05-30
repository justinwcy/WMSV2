namespace WMSCommon.Contracts.OrganizationService
{
    public class StaffUpdated<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
