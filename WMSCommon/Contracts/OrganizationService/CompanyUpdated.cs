namespace WMSCommon.Contracts.OrganizationService
{
    public class CompanyUpdated<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
