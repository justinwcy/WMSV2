using CatalogService.DbContexts;
using CatalogService.Models;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Repositories;
using WMSCommon.Services;
using Wolverine;

namespace CatalogService.Services
{
    public class ProductDetailService(
        IGenericRepository<IProductDetail> repository,
        IMessageBus publishEndpoint) : 
        GenericSyncService<IProductDetail>(repository, publishEndpoint), 
        IProductDetailService
    {
    }
}
