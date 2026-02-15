using InboundService.Constants;

using WMSCommon.Entities;

namespace InboundService.Models
{
    public class IncomingDetail : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid IncomingId { get; set; }
        public Incoming Incoming { get; set; }
        public Guid ProductDetailId { get; set; }
        public IncomingStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
