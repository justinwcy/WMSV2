using WMSCommon.Entities;

namespace WMSCommon.Contracts.OrganizationService;

public class ICompany : ISyncEntity, IGenericEntity
{
    public int Version { get; set; }
    public bool IsDeleted { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
}