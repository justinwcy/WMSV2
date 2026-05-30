using SalesService.DbContexts;
using SalesService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace SalesService.Repositories
{
    public class ProductDetailRepository(
        SalesDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<ProductDetail, SalesDbContext>(dbContext, userContext), IProductDetailRepository
    {
    }
}