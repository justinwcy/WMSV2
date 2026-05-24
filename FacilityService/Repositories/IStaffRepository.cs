using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IStaffRepository : ITenantRepository<Staff>
    {
        public Task<RepositoryResult<Staff>> AddRackIdsToStaffAsync(Guid staffId, IEnumerable<Guid> rackIds);
        public Task<RepositoryResult<Staff>> AddWarehouseIdsToStaffAsync(Guid staffId, IEnumerable<Guid> warehouseIds);
    }
}