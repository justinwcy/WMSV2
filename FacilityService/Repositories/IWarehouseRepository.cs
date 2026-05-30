using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IWarehouseRepository : ITenantRepository<Warehouse>
    {
        public Task<RepositoryResult<Warehouse>> CreateAsync(
            Warehouse warehouse, 
            IEnumerable<Guid> rackIds, 
            IEnumerable<Guid> staffIds);
        public Task<RepositoryResult<Warehouse>> UpdateAsync(
            Warehouse warehouse, 
            IEnumerable<Guid> rackIds, 
            IEnumerable<Guid> staffIds);
    }
}