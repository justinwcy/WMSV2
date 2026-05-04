namespace WMSCommon.Contracts
{
    public interface ISyncEvent<out T> where T : ISyncEntity
    {
        public T Data { get; }
        public DateTime OccurredAt { get; set; }
    }
}
