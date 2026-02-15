using InboundService.Constants;
using WMSCommon.Entities;

namespace InboundService.Models
{
    public class Incoming : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime EstimatedReceivedDate { get; set; }
        public IncomingType Type { get; set; }
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public string PONumber { get; set; }
        public ICollection<IncomingDetail> IncomingDetails { get; set; }
    }
}
