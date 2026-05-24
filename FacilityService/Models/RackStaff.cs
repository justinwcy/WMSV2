using WMSCommon.Entities;

namespace FacilityService.Models;

public class RackStaff : ITenantEntity
{
    public Guid Id { get; set; }
    public Guid RackId { get; set; }
    public Rack Rack { get; set; }
    public Guid StaffId { get; set; }
    public Staff Staff { get; set; }
    public Guid CompanyId { get; set; }
}