using FulfilmentService.DbContexts;
using FulfilmentService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace FulfilmentService.Repositories
{
    public class OrderRepository(
        FulfilmentDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Order, FulfilmentDbContext>(dbContext, userContext), IOrderRepository
    {
    }
}