using FulfilmentService.DbContexts;
using FulfilmentService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace FulfilmentService.Repositories
{
    public class OrderDetailRepository(
        FulfilmentDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<OrderDetail, FulfilmentDbContext>(dbContext, userContext), IOrderDetailRepository
    {
    }
}