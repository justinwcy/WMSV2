using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace FacilityService.Repositories
{
    public class RackRepository(
        FacilityDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<Rack, FacilityDbContext>(dbContext, userContext), IRackRepository
    {
        public async Task<RepositoryResult<Rack>> AddStaffIdsToRackAsync(Guid rackId, IEnumerable<Guid> staffIds)
        {
            var foundRack = await DbContext.Racks
                .Include(r => r.Staffs)
                .FirstOrDefaultAsync(r => r.Id == rackId);
            if (foundRack == null)
            {
                return RepositoryResult<Rack>.Failure($"RackId = {rackId} not found");
            }

            var staffToAssign = await DbContext.Staffs
                .Where(s => staffIds.Contains(s.Id))
                .ToListAsync();

            foreach (var staff in staffToAssign)
            {
                if (foundRack.Staffs.All(x => x.Id != staff.Id))
                {
                    foundRack.Staffs.Add(staff);
                }
            }
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Rack>.Success(foundRack);
        }
    }
}
