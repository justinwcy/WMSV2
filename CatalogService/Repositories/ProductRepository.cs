using CatalogService.DbContexts;
using CatalogService.Models;

using Microsoft.EntityFrameworkCore;

using WMSCommon.Contexts;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace CatalogService.Repositories
{
    public class ProductRepository(
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        IUserContext userContext) : 
        TenantRepository<Product, CatalogDbContext>(dbContextFactory, userContext), IProductRepository
    {
        public override async Task<RepositoryResult<Product>> UpdateAsync(Product entity)
        {
            await using CatalogDbContext dbContext = await dbContextFactory.CreateDbContextAsync();
            Product? existingProduct = await dbContext.Products
                .Include(p => p.Details)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == entity.Id && p.CompanyId == userContext.CompanyId);

            if (existingProduct == null)
            {
                return RepositoryResult<Product>.Failure("Product not found");
            }

            existingProduct.Name = entity.Name;
            existingProduct.Description = entity.Description;
            existingProduct.Tags = entity.Tags;
            dbContext.Products.Update(existingProduct);

            await dbContext.SaveChangesAsync();
            return RepositoryResult<Product>.Success(existingProduct);
        }
    }
}
