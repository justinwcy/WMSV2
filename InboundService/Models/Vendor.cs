using WMSCommon.Entities;

namespace InboundService.Models
{
    public class Vendor : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }
        public ICollection<InboundOrder> InboundOrders { get; set; }
    }
}
