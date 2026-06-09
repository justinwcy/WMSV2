namespace WMSCommon.Contracts.FulfilmentService
{
    public class OrderDetailDeleted<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
