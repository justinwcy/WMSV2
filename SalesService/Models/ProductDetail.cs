using WMSCommon.Contracts;
using WMSCommon.Contracts.CatalogService;

namespace SalesService.Models;

public class ProductDetail : IProductDetail
{
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Sku { get; set; }
    public double WeightKg { get; set; }
    public Dimensions ProductDimensions { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ImageToShow { get; set; }
    public Shop? Shop { get; set; }
    public Guid? ShopId { get; set; }
}