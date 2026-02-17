namespace WMSCommon.Contracts.CatalogService
{
    public interface IProductDetail : ISyncEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
        public double WeightKg { get; set; }
        public Dimensions ProductDimensions { get; set; }
        public Guid ProductId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
