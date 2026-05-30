using SalesService.Models;
using WMSCommon.Repositories;

namespace SalesService.Repositories;

public interface IShopRepository : ITenantRepository<Shop>
{
}