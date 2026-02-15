using CatalogService.Models;
using WMSCommon.Repositories;

namespace CatalogService.Repositories
{
    public interface IProductRepository : ITenantRepository<Product>
    {
    }
}
