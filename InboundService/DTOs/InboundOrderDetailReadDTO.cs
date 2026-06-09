using InboundService.Constants;
using InboundService.Models;

namespace InboundService.DTOs
{
    public class InboundOrderDetailReadDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid InboundOrderId { get; set; }
        public Guid ProductDetailId { get; set; }
        public ProductDetailReadDTO ProductDetail { get; set; }
        public IncomingStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
