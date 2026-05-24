using FacilityService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public interface IWarehouseRepository : ITenantRepository<Warehouse>
    {
        public Task<RepositoryResult<Warehouse>> AddStaffIdsToWarehouseAsync(Guid warehouseId, IEnumerable<Guid> staffIds);
        public Task<RepositoryResult<Warehouse>> AddRackIdsToWarehouseAsync(Guid warehouseId, IEnumerable<Guid> rackIds);
    }
}