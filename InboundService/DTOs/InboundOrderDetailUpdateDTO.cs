using InboundService.Constants;

namespace InboundService.DTOs
{
    public class InboundOrderDetailUpdateDTO
    {
        public Guid InboundOrderId { get; set; }
        public Guid ProductDetailId { get; set; }
        public IncomingStatus Status { get; set; }
        public int Quantity { get; set; }
    }
}
