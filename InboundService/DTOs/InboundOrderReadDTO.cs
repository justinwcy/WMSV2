using InboundService.Constants;

namespace InboundService.DTOs
{
    public class InboundOrderReadDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime EstimatedReceivedDate { get; set; }
        public InboundSource Source { get; set; }
        public Guid VendorId { get; set; }
        public string PONumber { get; set; }
        public IEnumerable<InboundOrderDetailReadDTO> IncomingDetails { get; set; }
    }
}
