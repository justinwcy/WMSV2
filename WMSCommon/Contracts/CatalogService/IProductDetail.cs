using WMSCommon.Entities;

namespace WMSCommon.Contracts.CatalogService
{
    public interface IProductDetail : ISyncEntity, ITenantEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public double WeightKg { get; set; }
        public Dimensions ProductDimensions { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ImageToShow { get; set; }
    }
}
