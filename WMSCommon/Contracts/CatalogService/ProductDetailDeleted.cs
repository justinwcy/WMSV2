namespace WMSCommon.Contracts.CatalogService
{
    public class ProductDetailDeleted<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
