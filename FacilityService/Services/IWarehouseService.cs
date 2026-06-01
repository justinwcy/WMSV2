using FacilityService.Models;
using WMSCommon.Contracts;
using WMSCommon.Results;
using WMSCommon.Services;

namespace FacilityService.Services
{
    public interface IWarehouseService : ITenantSyncService<Warehouse>
    {
        public Task<RepositoryResult<Warehouse>> CreateAndPublishAsync(
            Warehouse entity,
            IEnumerable<Guid> rackIds,
            IEnumerable<Guid> staffIds);

        public Task<RepositoryResult<Warehouse>> UpdateAndPublishAsync(
            Warehouse entity,
            IEnumerable<Guid> rackIds,
            IEnumerable<Guid> staffIds);
    }
}