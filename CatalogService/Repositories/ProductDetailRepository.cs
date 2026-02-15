using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace CatalogService.Repositories
{
    public class ProductDetailRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) :
        TenantRepository<ProductDetail, CatalogDbContext>(dbContextFactory, userContext), IProductDetailRepository
    {
        public override async Task<RepositoryResult<ProductDetail>> UpdateAsync(ProductDetail entity)
        {
            await using CatalogDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            ProductDetail? existingProductDetail = await dbContext.ProductDetails
                .FirstOrDefaultAsync(p => p.Id == entity.Id && p.CompanyId == userContext.CompanyId);

            if (existingProductDetail == null)
            {
                return RepositoryResult<ProductDetail>.Failure("Product Detail not found");
            }

            existingProductDetail.Name = entity.Name;
            existingProductDetail.Price = entity.Price;
            existingProductDetail.Sku = entity.Sku;
            existingProductDetail.WeightKg = entity.WeightKg;

            dbContext.ProductDetails.Update(existingProductDetail);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<ProductDetail>.Success(existingProductDetail);
        }
    }
}
