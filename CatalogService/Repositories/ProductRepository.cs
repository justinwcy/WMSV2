using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductRepository(
        CatalogDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Product, CatalogDbContext>(dbContext, userContext), IProductRepository
    {
    }
}
