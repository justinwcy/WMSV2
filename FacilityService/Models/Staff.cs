using WMSCommon.Contracts.OrganizationService;
using WMSCommon.Entities;

namespace FacilityService.Models;

public class Staff : IStaff
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public Guid CompanyId { get; set; }
    public ICollection<Warehouse> Warehouses { get; set; }
    public ICollection<Rack> Racks { get; set; }
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
}