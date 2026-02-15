namespace CatalogService.DTOs
{
    public class ProductImageCreateDTO
    {
        public Guid ProductId { get; set; }
        public string ImageBase64 { get; set; }
    }
}
