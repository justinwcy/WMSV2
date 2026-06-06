using Microsoft.EntityFrameworkCore;
using SalesService.DbContexts;
using SalesService.Models;
using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace SalesService.Repositories
{
    public class ShopRepository(
        SalesDbContext dbContext,
        IUserContext userContext) : 
        TenantRepository<Shop, SalesDbContext>(dbContext, userContext), IShopRepository
    {
        public async Task<RepositoryResult<Shop>> CreateAsync(Shop shop, IEnumerable<Guid> productDetailIds)
        {
            shop.CompanyId = UserContext.CompanyId;
            await AssignEntitiesAsync(productDetailIds, DbContext.ProductDetails, shop.ProductDetails);
            
            await DbContext.Shops.AddAsync(shop);
            await DbContext.SaveChangesAsync();
            DbContext.Entry(shop).State = EntityState.Detached;
            return RepositoryResult<Shop>.Success(shop);
        }

        public async Task<RepositoryResult<Shop>> UpdateAsync(Shop shop, IEnumerable<Guid> productDetailIds)
        {
            var query = DbContext.Shops
                .Include(s => s.ProductDetails);
            var repositoryResult = await GetAndValidateAsync(shop.Id, query);
            if (!repositoryResult.IsSuccess)
            {
                return repositoryResult;
            }
            
            var existing = repositoryResult.Data!;
            
            // update the existing rack
            DbContext.Entry(existing).CurrentValues.SetValues(shop);
            existing.CompanyId = UserContext.CompanyId;
            await AddIdsToCollectionAsync(existing.ProductDetails, productDetailIds, DbContext.ProductDetails);
            
            await DbContext.SaveChangesAsync();
            return RepositoryResult<Shop>.Success(existing);
        }
    }
}