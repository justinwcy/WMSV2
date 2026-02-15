using InboundService.Constants;

using WMSCommon.Entities;

namespace InboundService.Models
{
    public class InboundOrderDetail : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid IncomingId { get; set; }
        public InboundOrder Incoming { get; set; }
        public Guid ProductDetailId { get; set; }
        public IncomingStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
