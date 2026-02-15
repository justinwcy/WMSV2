using WMSCommon.Entities;

namespace CatalogService.Models
{
    public class Product : ITenantEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public ICollection<ProductDetail> Details { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public Guid CompanyId { get; set; }
    }
}
