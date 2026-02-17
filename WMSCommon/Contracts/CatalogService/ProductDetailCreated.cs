namespace WMSCommon.Contracts.CatalogService
{
    public class ProductDetailCreated : ISyncEvent<IProductDetail>
    {
        public IProductDetail Data { get; set; }
        public DateTime OccurredAt { get; set; }
        public void MapFrom(IProductDetail entity)
        {
            Data = entity;
        }
    }
}
