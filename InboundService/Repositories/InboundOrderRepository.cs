using InboundService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace InboundService.Repositories
{
    public class InboundOrderRepository(
        InboundDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<InboundOrder, InboundDbContext>(dbContext, userContext), IInboundOrderRepository
    {
    }
}
