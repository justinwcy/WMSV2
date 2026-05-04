using CatalogService.DbContexts;
using CatalogService.Models;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Repositories;
using WMSCommon.Services;
using Wolverine;

namespace CatalogService.Services
{
    public class ProductDetailService(
        ITenantRepository<ProductDetail> repository,
        IMessageBus publishEndpoint) : 
        TenantSyncService<ProductDetail>(repository, publishEndpoint), 
        IProductDetailService
    {
    }
}
