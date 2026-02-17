using CatalogService.DbContexts;
using InboundService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace InboundService.Repositories
{
    public class InboundOrderRepository(
        IDbContextFactory<InboundDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<InboundOrder, InboundDbContext>(dbContextFactory, userContext), IInboundOrderRepository
    {
    }
}
