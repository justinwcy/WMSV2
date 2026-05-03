using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductDetailRepository(
        CatalogDbContext dbContext,
        IUserContext userContext) :
        TenantRepository<ProductDetail, CatalogDbContext>(dbContext, userContext), IProductDetailRepository
    {
    }
}
