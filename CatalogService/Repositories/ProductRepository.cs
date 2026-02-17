using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<Product, CatalogDbContext>(dbContextFactory, userContext), IProductRepository
    {
    }
}
