using WMSCommon.Entities;

namespace WMSCommon.Contracts.OrganizationService;

public interface IStaff : ISyncEntity, ITenantEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}