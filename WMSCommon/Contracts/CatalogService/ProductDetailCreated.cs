namespace WMSCommon.Contracts.CatalogService
{
    public class ProductDetailCreated<T> : ISyncEvent<T> where T : ISyncEntity
    {
        public T Data { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}