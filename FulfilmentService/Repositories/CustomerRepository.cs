using FulfilmentService.DbContexts;
using FulfilmentService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace FulfilmentService.Repositories
{
    public class CustomerRepository(
        FulfilmentDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Customer, FulfilmentDbContext>(dbContext, userContext), ICustomerRepository
    {
    }
}