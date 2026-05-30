using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IRackRepository : ITenantRepository<Rack>
    {
        public Task<RepositoryResult<Rack>> CreateAsync(Rack rack, IEnumerable<Guid> staffIds);
        
        public Task<RepositoryResult<Rack>> UpdateAsync(Rack rack, IEnumerable<Guid> staffIds);
    }
}