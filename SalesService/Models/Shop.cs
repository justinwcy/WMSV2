using WMSCommon.Entities;

namespace SalesService.Models;

public class Shop : ITenantEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public ICollection<ProductDetail> ProductDetails { get; set; }
}