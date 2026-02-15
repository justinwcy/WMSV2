using WMSCommon.Entities;

namespace CatalogService.Models
{
    public class ProductImage : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string ImageBase64 { get; set; }
        public Guid CompanyId { get; set; }
    }
}
