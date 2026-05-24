using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Services;
using Wolverine;

namespace FacilityService.Services
{
    public class WarehouseService(
        ITenantRepository<Warehouse> repository,
        IMessageBus publishEndpoint) : 
        TenantSyncService<Warehouse>(repository, publishEndpoint), 
        IWarehouseService
    {
    }
}
