using FacilityService.Models;
using FacilityService.Repositories;
using WMSCommon.Contracts.FacilityService;
using WMSCommon.Results;
using WMSCommon.Services;
using Wolverine;

namespace FacilityService.Services
{
    public class RackService(
        IRackRepository repository,
        IMessageContext messageContext) : 
        TenantSyncService<Rack>(repository, messageContext), 
        IRackService
    {
        public async Task<RepositoryResult<Rack>> CreateAndPublishAsync(Rack entity, IEnumerable<Guid> staffIds)
        {
            return await ExecuteWithTransaction<RackCreated<Rack>>(
                async () => await repository.CreateAsync(entity, staffIds));
        }

        public async Task<RepositoryResult<Rack>> UpdateAndPublishAsync(Rack entity, IEnumerable<Guid> staffIds)
        {
            return await ExecuteWithTransaction<RackUpdated<Rack>>(
                async () => await repository.UpdateAsync(entity, staffIds));
        }
    }
}
