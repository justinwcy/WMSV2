using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public class ProductImageRepository(
        CatalogDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<ProductImage, CatalogDbContext>(dbContext, userContext), 
        IProductImageRepository
    {
    }
}
