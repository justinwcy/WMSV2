using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductDetailRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) :
        TenantRepository<ProductDetail, CatalogDbContext>(dbContextFactory, userContext), IProductDetailRepository
    {
    }
}
