namespace CatalogService.DTOs
{
    public class ProductDetailCreateDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Sku { get; set; }

        public double WeightKg { get; set; }

        public DimensionsDTO ProductDimensions { get; set; }
        public Guid? ImageToShow { get; set; }
        public Guid ProductId { get; set; }
    }
}
