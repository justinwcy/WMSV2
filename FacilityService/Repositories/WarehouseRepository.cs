using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public class WarehouseRepository(
        FacilityDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<Warehouse, FacilityDbContext>(dbContext, userContext), IWarehouseRepository
    {
        public async Task<RepositoryResult<Warehouse>> CreateAsync(
            Warehouse warehouse, 
            IEnumerable<Guid> rackIds, 
            IEnumerable<Guid> staffIds)
        {
            warehouse.Version = 0;
            warehouse.CompanyId = UserContext.CompanyId;
            
            await AssignEntitiesAsync(staffIds, DbContext.Staffs, warehouse.Staffs);
            await AssignEntitiesAsync(rackIds, DbContext.Racks, warehouse.Racks);
            
            await DbContext.Warehouses.AddAsync(warehouse);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(warehouse).State = EntityState.Detached;
            return RepositoryResult<Warehouse>.Success(warehouse);
        }
        
        public async Task<RepositoryResult<Warehouse>> UpdateAsync(Warehouse warehouse, IEnumerable<Guid> rackIds, IEnumerable<Guid> staffIds)
        {
            var query = DbContext.Warehouses
                .Include(w => w.Racks)
                .Include(w => w.Staffs);
            var repositoryResult = await GetAndValidateAsync(warehouse.Id, query);
            if (!repositoryResult.IsSuccess)
            {
                return repositoryResult;
            }
            
            var existing = repositoryResult.Data!;
            var versionNumber = existing.Version;
            
            // update the existing rack
            DbContext.Entry(existing).CurrentValues.SetValues(warehouse);
            existing.Version = versionNumber + 1;
            existing.CompanyId = UserContext.CompanyId;
            await AddIdsToCollectionAsync(existing.Racks, rackIds, DbContext.Racks);
            await AddIdsToCollectionAsync(existing.Staffs, staffIds, DbContext.Staffs);
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Warehouse>.Success(existing);
        }
    }
}
