using SalesService.Models;
using WMSCommon.Repositories;

namespace SalesService.Repositories;

public interface IProductDetailRepository : ITenantRepository<ProductDetail>
{
}