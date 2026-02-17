using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductImageRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) :
        TenantRepository<ProductImage, CatalogDbContext>(dbContextFactory, userContext), IProductImageRepository
    {
    }
}
