using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace CatalogService.Repositories
{
    public class ProductImageRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) :
        TenantRepository<ProductImage, CatalogDbContext>(dbContextFactory, userContext), IProductImageRepository
    {
        public override async Task<RepositoryResult<ProductImage>> UpdateAsync(ProductImage entity)
        {
            await using CatalogDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            ProductImage? existingProductImage = await dbContext.ProductImages
                .FirstOrDefaultAsync(p => p.Id == entity.Id && p.CompanyId == userContext.CompanyId);

            if (existingProductImage == null)
            {
                return RepositoryResult<ProductImage>.Failure("Product Image not found");
            }

            existingProductImage.ImageBase64 = entity.ImageBase64;
            dbContext.ProductImages.Update(existingProductImage);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<ProductImage>.Success(existingProductImage);
        }
    }
}
