namespace CatalogService.DTOs
{
    public class ProductImageReadDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ImageBase64 { get; set; }
        public Guid CompanyId { get; set; }
    }
}
