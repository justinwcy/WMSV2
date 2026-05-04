using InboundService.DbContexts;
using InboundService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace InboundService.Repositories
{
    public class VendorRepository(
        InboundDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Vendor, InboundDbContext>(dbContext, userContext), IVendorRepository
    {
    }
}
