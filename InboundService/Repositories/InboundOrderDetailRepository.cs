using InboundService.DbContexts;
using InboundService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;

namespace InboundService.Repositories
{
    public class InboundOrderDetailRepository(
        InboundDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<InboundOrderDetail, InboundDbContext>(dbContext, userContext), IInboundOrderDetailRepository
    {
    }
}
