using InboundService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.CatalogService;

namespace InboundService.Consumers;

public class ProductDetailDeletedConsumer(InboundDbContext dbContext)
{
    public async Task Handle(ProductDetailDeleted<ProductDetail> message)
    {
        var existingProduct = await dbContext.ProductDetails
            .FirstOrDefaultAsync(x => x.Id == message.Data.Id);

        if (existingProduct != null)
        {
            if (message.Data.Version <= existingProduct.Version)
            {
                return; 
            }
            
            existingProduct.IsDeleted = true;
            existingProduct.Version = message.Data.Version;
            
            await dbContext.SaveChangesAsync();
        }
    }
}