using WMSCommon.Entities;

namespace WMSCommon.Contracts.FacilityService;

public interface IWarehouse : ISyncEntity, ITenantEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
}