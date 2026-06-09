using WMSCommon.Entities;

namespace WMSCommon.Contracts.FulfilmentService;

public interface IOrderDetail : ISyncEntity, ITenantEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
}