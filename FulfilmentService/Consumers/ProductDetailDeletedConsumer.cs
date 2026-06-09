using FulfilmentService.DbContexts;
using FulfilmentService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.CatalogService;

namespace FulfilmentService.Consumers;

public class ProductDetailDeletedConsumer(FulfilmentDbContext dbContext)
{
    public async Task Handle(ProductDetailDeleted<ProductDetail> message)
    {
        var existingProduct = await dbContext.ProductDetails
            .FirstOrDefaultAsync(x => x.Id == message.Data.Id);

        if (existingProduct != null)
        {
            existingProduct.IsDeleted = true;
            existingProduct.Version = message.Data.Version;
            
            await dbContext.SaveChangesAsync();
        }
    }
}