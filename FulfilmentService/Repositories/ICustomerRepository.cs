using FulfilmentService.Models;
using WMSCommon.Repositories;

namespace FulfilmentService.Repositories;

public interface ICustomerRepository : ITenantRepository<Customer>
{
}