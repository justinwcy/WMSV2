using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Services;
using Wolverine;

namespace FacilityService.Services
{
    public class RackService(
        ITenantRepository<Rack> repository,
        IMessageBus publishEndpoint) : 
        TenantSyncService<Rack>(repository, publishEndpoint), 
        IRackService
    {
    }
}
