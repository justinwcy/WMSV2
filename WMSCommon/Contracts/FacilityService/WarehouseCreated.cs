namespace WMSCommon.Contracts.FacilityService
{
    public class WarehouseCreated<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}