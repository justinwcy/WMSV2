using Microsoft.EntityFrameworkCore;
using SalesService.DbContexts;
using SalesService.Models;
using WMSCommon.Contracts.CatalogService;

namespace SalesService.Consumers;

public class ProductDetailUpdatedConsumer(SalesDbContext dbContext)
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