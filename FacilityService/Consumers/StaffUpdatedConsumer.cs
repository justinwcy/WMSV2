using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.OrganizationService;

namespace FacilityService.Consumers;

public class StaffUpdatedConsumer(FacilityDbContext dbContext)
{
    public async Task Handle(StaffUpdated<Staff> message)
    {
        var existingProduct = await dbContext.Staffs
            .FirstOrDefaultAsync(x => x.Id == message.Data.Id);

        if (existingProduct != null)
        {
            if (message.Data.Version <= existingProduct.Version)
            {
                return; 
            }
            
            dbContext.Entry(existingProduct).CurrentValues.SetValues(message.Data);
            await dbContext.SaveChangesAsync();
        }
    }
}