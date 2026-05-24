namespace WMSCommon.Contracts.FacilityService
{
    public class RackUpdated<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}