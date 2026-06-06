using FacilityService.DbContexts;
using FacilityService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.OrganizationService;

namespace FacilityService.Consumers;

public class StaffCreatedConsumer(FacilityDbContext dbContext)
{
    public async Task Handle(StaffCreated<Staff> message)
    {
        var exists = await dbContext.Staffs
            .AnyAsync(x => x.Id == message.Data.Id);

        if (!exists)
        {
            dbContext.Staffs.Add(new Staff()
            {
                CompanyId =  message.Data.CompanyId,
                Id =  message.Data.Id,
                Racks = new List<Rack>(),
                Warehouses = new List<Warehouse>(),
                Email = message.Data.Email,
                FirstName = message.Data.FirstName,
                LastName = message.Data.LastName,
                UserName = message.Data.UserName,
                IsDeleted = false,
                Version = 0,
            });

            await dbContext.SaveChangesAsync();
        }
    }
}