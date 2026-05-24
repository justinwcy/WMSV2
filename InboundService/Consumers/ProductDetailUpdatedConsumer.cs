using InboundService.DbContexts;
using InboundService.Models;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contracts.CatalogService;

namespace InboundService.Consumers;

public class ProductDetailUpdatedConsumer(InboundDbContext dbContext)
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
            
            existingProduct.CompanyId = message.Data.CompanyId;
            existingProduct.ProductId = message.Data.ProductId;
            existingProduct.Price = message.Data.Price;
            existingProduct.Name = message.Data.Name;
            existingProduct.ProductDimensions = message.Data.ProductDimensions;
            existingProduct.ImageToShow = message.Data.ImageToShow;
            existingProduct.Sku = message.Data.Sku;
            existingProduct.WeightKg = message.Data.WeightKg;
            existingProduct.Version = message.Data.Version;
            
            await dbContext.SaveChangesAsync();
        }
    }
}