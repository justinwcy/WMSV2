using WMSCommon.Contracts;
using WMSCommon.Contracts.FulfilmentService;
using WMSCommon.Entities;

namespace FulfilmentService.Models;

public class OrderDetail : IOrderDetail
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid ProductDetailId { get; set; }
    public ProductDetail ProductDetail { get; set; }
    public int Quantity { get; set; }
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
}