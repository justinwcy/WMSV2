using InboundService.Constants;
using WMSCommon.Entities;

namespace InboundService.Models
{
    public class InboundOrder : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime EstimatedReceivedDate { get; set; }
        public InboundSource Source { get; set; }
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public string PONumber { get; set; }
        public ICollection<InboundOrderDetail> IncomingDetails { get; set; }
    }
}
