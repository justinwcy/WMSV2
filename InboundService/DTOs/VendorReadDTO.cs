namespace InboundService.DTOs
{
    public class VendorReadDTO
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public IEnumerable<InboundOrderReadDTO> InboundOrders { get; set; }
    }
}
