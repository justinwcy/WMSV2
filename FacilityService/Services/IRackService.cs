using FacilityService.Models;
using WMSCommon.Results;
using WMSCommon.Services;

namespace FacilityService.Services
{
    public interface IRackService : ITenantSyncService<Rack>
    {
        public Task<RepositoryResult<Rack>> CreateAndPublishAsync(
            Rack entity,
            IEnumerable<Guid> staffIds);

        public Task<RepositoryResult<Rack>> UpdateAndPublishAsync(
            Rack entity,
            IEnumerable<Guid> staffIds);
    }
}