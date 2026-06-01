using FacilityService.Models;
using FacilityService.Repositories;
using WMSCommon.Contracts.FacilityService;
using WMSCommon.Results;
using WMSCommon.Services;
using Wolverine;

namespace FacilityService.Services
{
    public class WarehouseService(
        IWarehouseRepository repository,
        IMessageContext messageContext) : 
        TenantSyncService<Warehouse>(repository, messageContext), 
        IWarehouseService
    {
        public async Task<RepositoryResult<Warehouse>> CreateAndPublishAsync(Warehouse entity, IEnumerable<Guid> rackIds, IEnumerable<Guid> staffIds)
        {
            return await ExecuteWithTransaction<WarehouseCreated<Warehouse>>(
                async () => await repository.CreateAsync(entity,  rackIds, staffIds));
        }

        public async Task<RepositoryResult<Warehouse>> UpdateAndPublishAsync(Warehouse entity, IEnumerable<Guid> rackIds, IEnumerable<Guid> staffIds)
        {
            return await ExecuteWithTransaction<WarehousesUpdated<Warehouse>>(
                async () => await repository.CreateAsync(entity,  rackIds, staffIds));
        }
    }
}
