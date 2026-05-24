using WMSCommon.Entities;

namespace FacilityService.Models;

public class WarehouseStaff : ITenantEntity
{
    public Guid Id { get; set; }
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }
    public Guid StaffId { get; set; }
    public Staff Staff { get; set; }
    public Guid CompanyId { get; set; }
}