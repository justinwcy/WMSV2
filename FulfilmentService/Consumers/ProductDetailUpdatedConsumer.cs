using FulfilmentService.DbContexts;
using FulfilmentService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.CatalogService;

namespace FulfilmentService.Consumers;

public class ProductDetailUpdatedConsumer(FulfilmentDbContext dbContext)
{
    public async Task Handle(ProductDetailUpdated<ProductDetail> message)
    {
        var existingProduct = await dbContext.ProductDetails
            .FirstOrDefaultAsync(x => x.Id == message.Data.Id);

        if (existingProduct != null)
        {
            if (message.Data.Version <= existingProduct.Version)
            {
                return; 
            }
            dbContext.Entry(existingProduct).CurrentValues.SetValues(message.Data);
            await dbContext.SaveChangesAsync();
        }
    }
}