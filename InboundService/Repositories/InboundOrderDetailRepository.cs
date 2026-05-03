using InboundService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace InboundService.Repositories
{
    public class InboundOrderDetailRepository(
        IDbContextFactory<InboundDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<InboundOrderDetail, InboundDbContext>(dbContextFactory, userContext), IInboundOrderDetailRepository
    {
    }
}
