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
        public async Task<RepositoryResult<Warehouse>> AddStaffIdsToWarehouseAsync(Guid warehouseId, IEnumerable<Guid> staffIds)
        {
            var foundWarehouse = await DbContext.Warehouses
                .Include(w => w.Staffs)
                .FirstOrDefaultAsync(w => w.Id == warehouseId);
            if (foundWarehouse == null)
            {
                return RepositoryResult<Warehouse>.Failure($"WarehouseId = {warehouseId} not found");
            }

            var staffsToAssign = await DbContext.Staffs
                .Where(x => staffIds.Contains(x.Id))
                .ToListAsync();
            foreach (var staff in staffsToAssign)
            {
                if (foundWarehouse.Staffs.All(x => x.Id != staff.Id))
                {
                    foundWarehouse.Staffs.Add(staff);
                }
            }
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Warehouse>.Success(foundWarehouse);
        }

        public async Task<RepositoryResult<Warehouse>> AddRackIdsToWarehouseAsync(Guid warehouseId, IEnumerable<Guid> rackIds)
        {
            var foundWarehouse = await DbContext.Warehouses
                .Include(w => w.Racks)
                .FirstOrDefaultAsync(w => w.Id == warehouseId);
            if (foundWarehouse == null)
            {
                return RepositoryResult<Warehouse>.Failure($"WarehouseId = {warehouseId} not found");
            }

            var racksToAssign = await DbContext.Racks
                .Where(x => rackIds.Contains(x.Id))
                .ToListAsync();
            foreach (var rack in racksToAssign)
            {
                if (foundWarehouse.Racks.All(x => x.Id != rack.Id))
                {
                    foundWarehouse.Racks.Add(rack);
                }
            }
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Warehouse>.Success(foundWarehouse);
        }
    }
}
