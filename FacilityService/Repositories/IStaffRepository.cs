using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IStaffRepository : ITenantRepository<Staff>
    {
        public Task<RepositoryResult<Staff>> CreateAsync(Staff staff, IEnumerable<Guid> rackIds);
        public Task<RepositoryResult<Staff>> UpdateAsync(Staff staff, IEnumerable<Guid> rackIds);
    }
}