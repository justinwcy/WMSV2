using WMSCommon.Entities;

namespace FulfilmentService.Models;

public class Order : ITenantEntity
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedArrivalDate { get; set; }
    public string Address { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}