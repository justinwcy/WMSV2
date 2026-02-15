namespace CatalogService.DTOs
{
    public class ProductImageUpdateDTO
    {
        public Guid ProductId { get; set; }
        public string ImageBase64 { get; set; }
    }
}
