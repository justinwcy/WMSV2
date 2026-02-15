namespace CatalogService.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
