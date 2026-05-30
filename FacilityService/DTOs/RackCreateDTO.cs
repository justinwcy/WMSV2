namespace FacilityService.DTOs;

public class RackCreateDTO
{
    public Guid WarehouseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double WeightKg { get; set; }
    public double HeightMM { get; set; }
    public double WidthMM { get; set; }
    public double DepthMM { get; set; }
    public IEnumerable<Guid> StaffIds { get; set; }
}