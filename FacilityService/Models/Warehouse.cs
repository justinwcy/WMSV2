using WMSCommon.Contracts.FacilityService;
using WMSCommon.Entities;

namespace FacilityService.Models;

public class Warehouse : IWarehouse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public ICollection<Rack> Racks { get; set; }
    public ICollection<Staff> Staffs { get; set; }
    public Guid CompanyId { get; set; }
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
}