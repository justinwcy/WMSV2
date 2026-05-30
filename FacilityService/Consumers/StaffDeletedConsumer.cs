using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Contracts.OrganizationService;

namespace FacilityService.Consumers;

public class StaffDeletedConsumer(FacilityDbContext dbContext)
{
    public async Task Handle(StaffDeleted<Staff> message)
    {
        var existingProduct = await dbContext.Staffs
            .FirstOrDefaultAsync(x => x.Id == message.Data.Id);

        if (existingProduct != null)
        {
            if (message.Data.Version <= existingProduct.Version)
            {
                return; 
            }
            
            existingProduct.IsDeleted = true;
            existingProduct.Version = message.Data.Version;
            
            await dbContext.SaveChangesAsync();
        }
    }
}