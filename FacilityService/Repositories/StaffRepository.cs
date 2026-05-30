using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Entities;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public class StaffRepository(
        FacilityDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<Staff, FacilityDbContext>(dbContext, userContext), IStaffRepository
    {
        public async Task<RepositoryResult<Staff>> CreateAsync(Staff staff, IEnumerable<Guid> rackIds)
        {
            staff.Version = 0;
            staff.CompanyId = UserContext.CompanyId;
            
            await AssignEntitiesAsync(rackIds, DbContext.Racks, staff.Racks);
           
            await DbContext.Staffs.AddAsync(staff);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(staff).State = EntityState.Detached;
            return RepositoryResult<Staff>.Success(staff);
        }
        
        public async Task<RepositoryResult<Staff>> UpdateAsync(Staff staff, IEnumerable<Guid> rackIds)
        {
            var query = DbContext.Staffs
                .Include(s => s.Racks);
            var repositoryResult = await GetAndValidateAsync(staff.Id, query);
            if (!repositoryResult.IsSuccess)
            {
                return repositoryResult;
            }
            
            var existing = repositoryResult.Data!;
            var versionNumber = existing.Version;
            
            // update the existing rack
            DbContext.Entry(existing).CurrentValues.SetValues(staff);
            existing.Version = versionNumber + 1;
            existing.CompanyId = UserContext.CompanyId;
            await AddIdsToCollectionAsync(existing.Racks, rackIds, DbContext.Racks);
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Staff>.Success(existing);
        }
    }
}
