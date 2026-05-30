using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Entities;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public class RackRepository(
        FacilityDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<Rack, FacilityDbContext>(dbContext, userContext), IRackRepository
    {
        public async Task<RepositoryResult<Rack>> CreateAsync(Rack rack, IEnumerable<Guid> staffIds)
        {
            rack.Version = 1;
            rack.CompanyId = UserContext.CompanyId;

            await AssignEntitiesAsync(staffIds, DbContext.Staffs, rack.Staffs);
            
            await DbContext.Racks.AddAsync(rack);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(rack).State = EntityState.Detached;
            return RepositoryResult<Rack>.Success(rack);
        }

        public async Task<RepositoryResult<Rack>> UpdateAsync(Rack rack, IEnumerable<Guid> staffIds)
        {
            var query = DbContext.Racks
                .Include(r => r.Staffs);
            var repositoryResult = await GetAndValidateAsync(rack.Id, query);
            if (!repositoryResult.IsSuccess)
            {
                return repositoryResult;
            }
            
            var existing = repositoryResult.Data!;
            existing.Version++;
            
            // update the existing rack
            DbContext.Entry(existing).CurrentValues.SetValues(rack);
            await AddIdsToCollectionAsync(existing.Staffs, staffIds, DbContext.Staffs);
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Rack>.Success(existing);
        }
    }
}
