using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public class StaffRepository(
        FacilityDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<Staff, FacilityDbContext>(dbContext, userContext), IStaffRepository
    {
        public async Task<RepositoryResult<Staff>> AddRackIdsToStaffAsync(Guid staffId, IEnumerable<Guid> rackIds)
        {
            var foundStaff = await DbContext.Staffs
                .Include(s => s.Racks)
                .FirstOrDefaultAsync(s => s.Id == staffId);
            if (foundStaff == null)
            {
                return RepositoryResult<Staff>.Failure($"StaffId = {staffId} not found");
            }

            var rackToAssign = await DbContext.Racks
                .Where(r => rackIds.Contains(r.Id))
                .ToListAsync();

            foreach (var rack in rackToAssign)
            {
                if (foundStaff.Racks.All(x => x.Id != rack.Id))
                {
                    foundStaff.Racks.Add(rack);
                }
            }
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Staff>.Success(foundStaff);
        }

        public async Task<RepositoryResult<Staff>> AddWarehouseIdsToStaffAsync(Guid staffId, IEnumerable<Guid> warehouseIds)
        {
            var foundStaff = await DbContext.Staffs
                .Include(s => s.Warehouses)
                .FirstOrDefaultAsync(s => s.Id == staffId);
            if (foundStaff == null)
            {
                return RepositoryResult<Staff>.Failure($"StaffId = {staffId} not found");
            }

            var warehouseToAssign = await DbContext.Warehouses
                .Where(w => warehouseIds.Contains(w.Id))
                .ToListAsync();

            foreach (var warehouse in warehouseToAssign)
            {
                if (foundStaff.Warehouses.All(x => x.Id != warehouse.Id))
                {
                    foundStaff.Warehouses.Add(warehouse);
                }
            }
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Staff>.Success(foundStaff);
        }
    }
}
