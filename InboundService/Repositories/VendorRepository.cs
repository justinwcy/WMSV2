using CatalogService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace InboundService.Repositories
{
    public class VendorRepository(
        IDbContextFactory<InboundDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<Vendor, InboundDbContext>(dbContextFactory, userContext), IVendorRepository
    {
        public override async Task<RepositoryResult<Vendor>> UpdateAsync(Vendor entity)
        {
            await using InboundDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            Vendor? existingVendor = await dbContext.Vendors
                .FirstOrDefaultAsync(p => p.Id == entity.Id && p.CompanyId == userContext.CompanyId);

            if (existingVendor == null)
            {
                return RepositoryResult<Vendor>.Failure("Vendor not found");
            }

            existingVendor.Name = entity.Name;
            existingVendor.Address = entity.Address;
            existingVendor.Email = entity.Email;
            existingVendor.Notes = entity.Notes;
            dbContext.Vendors.Update(existingVendor);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<Vendor>.Success(existingVendor);
        }
    }
}
