using CatalogService.DbContexts;
using CatalogService.Models;

using MassTransit;

using WMSCommon.Repositories;
using WMSCommon.Services;

namespace CatalogService.Services
{
    public class ProductDetailService(
        IGenericRepository<ProductDetail> repository,
        IPublishEndpoint publishEndpoint,
        CatalogDbContext dbContext)
        : GenericSyncService<ProductDetail, CatalogDbContext>(repository, publishEndpoint, dbContext),
            IProductDetailService
    {
    }
}
