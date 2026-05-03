using CatalogService.Models;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public interface IProductDetailRepository : ITenantRepository<IProductDetail>
    {
    }
}
