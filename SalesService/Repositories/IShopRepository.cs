using SalesService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace SalesService.Repositories;

public interface IShopRepository : ITenantRepository<Shop>
{
    public Task<RepositoryResult<Shop>> CreateAsync(Shop shop, IEnumerable<Guid> productDetailIds);
    public Task<RepositoryResult<Shop>> UpdateAsync(Shop shop, IEnumerable<Guid> productDetailIds);
}