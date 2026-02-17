using InboundService.DbContexts;
using InboundService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace InboundService.Repositories
{
    public class VendorRepository(
        IDbContextFactory<InboundDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<Vendor, InboundDbContext>(dbContextFactory, userContext), IVendorRepository
    {
    }
}
