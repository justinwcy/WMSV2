using CatalogService.Models;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Services;

namespace CatalogService.Services
{
    public interface IProductDetailService : ITenantSyncService<ProductDetail>
    {
    }
}
