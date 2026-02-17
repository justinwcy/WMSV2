namespace WMSCommon.Contracts
{
    public interface ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; }
        public void MapFrom(T entity);
    }
}
