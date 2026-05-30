namespace FacilityService.DTOs;

public class WarehouseCreateDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public IEnumerable<Guid> RackIds { get; set; }
    public IEnumerable<Guid> StaffIds { get; set; }
}