using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IRackRepository : ITenantRepository<Rack>
    {
        public Task<RepositoryResult<Rack>> AddStaffIdsToRackAsync(Guid rackId, IEnumerable<Guid> staffIds);
    }
}