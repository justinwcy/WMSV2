using WMSCommon.Contracts.FacilityService;
using WMSCommon.Entities;

namespace FacilityService.Models;

public class Rack : IRack
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Warehouse Warehouse { get; set; }
    public Guid WarehouseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double WeightKg { get; set; }
    public double HeightMM { get; set; }
    public double WidthMM { get; set; }
    public double DepthMM { get; set; }
    public ICollection<Staff> Staffs { get; set; }
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
}