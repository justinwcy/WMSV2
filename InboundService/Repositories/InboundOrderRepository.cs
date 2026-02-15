using CatalogService.DbContexts;
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
        public override async Task<RepositoryResult<InboundOrder>> UpdateAsync(InboundOrder entity)
        {
            await using InboundDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            InboundOrder? existingInboundOrder = await dbContext.InboundOrders
                .FirstOrDefaultAsync(p => p.Id == entity.Id && 
                                          p.CompanyId == userContext.CompanyId);

            if (existingInboundOrder == null)
            {
                return RepositoryResult<InboundOrder>.Failure("InboundOrder not found");
            }

            existingInboundOrder.IssuedDate = entity.IssuedDate;
            existingInboundOrder.ReceivedDate = entity.ReceivedDate;
            existingInboundOrder.EstimatedReceivedDate = entity.EstimatedReceivedDate;
            existingInboundOrder.Source = entity.Source;
            existingInboundOrder.VendorId = entity.VendorId;
            existingInboundOrder.PONumber = entity.PONumber;

            dbContext.InboundOrders.Update(existingInboundOrder);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<InboundOrder>.Success(existingInboundOrder);
        }
    }
}
