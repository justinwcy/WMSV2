using SalesService.DbContexts;
using SalesService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace SalesService.Repositories
{
    public class ShopRepository(
        SalesDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Shop, SalesDbContext>(dbContext, userContext), IShopRepository
    {
    }
}