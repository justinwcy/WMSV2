using InboundService.Constants;

using WMSCommon.Entities;

namespace InboundService.Models
{
    public class InboundOrderDetail : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid InboundOrderId { get; set; }
        public InboundOrder InboundOrder { get; set; }
        public Guid ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public IncomingStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
