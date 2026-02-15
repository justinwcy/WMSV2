using System.ComponentModel.DataAnnotations.Schema;
using WMSCommon.Entities;

namespace CatalogService.Models
{
    public class ProductDetail: ITenantEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Price { get; set; }

        public string Sku { get; set; }

        public double WeightKg { get; set; }

        public Dimensions ProductDimensions { get; set; }
        public Guid? ImageToShow { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid CompanyId { get; set; }
    }
}
