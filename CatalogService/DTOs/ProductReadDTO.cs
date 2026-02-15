namespace CatalogService.DTOs
{
    public class ProductReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public IEnumerable<ProductDetailReadDTO> Details { get; set; }
        public IEnumerable<ProductImageReadDTO> Images { get; set; }
        public Guid CompanyId { get; set; }
    }
}
