using CatalogService.DbContexts;
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
        public override async Task<RepositoryResult<InboundOrderDetail>> UpdateAsync(InboundOrderDetail entity)
        {
            await using InboundDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            InboundOrderDetail? existingInboundOrderDetail = await dbContext.InboundOrderDetails
                .FirstOrDefaultAsync(p => p.Id == entity.Id && p.CompanyId == userContext.CompanyId);

            if (existingInboundOrderDetail == null)
            {
                return RepositoryResult<InboundOrderDetail>.Failure("InboundOrderDetail not found");
            }

            existingInboundOrderDetail.InboundOrderId = entity.InboundOrderId;
            existingInboundOrderDetail.ProductDetailId = entity.ProductDetailId;
            existingInboundOrderDetail.Status = entity.Status;
            existingInboundOrderDetail.Quantity = entity.Quantity;

            dbContext.InboundOrderDetails.Update(existingInboundOrderDetail);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<InboundOrderDetail>.Success(existingInboundOrderDetail);
        }
    }
}
