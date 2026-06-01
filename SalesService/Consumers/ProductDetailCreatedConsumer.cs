using Microsoft.EntityFrameworkCore;
using SalesService.DbContexts;
using SalesService.Models;
using WMSCommon.Contracts.CatalogService;

namespace SalesService.Consumers;

public class ProductDetailCreatedConsumer(SalesDbContext dbContext)
{
    public async Task Handle(ProductDetailCreated<ProductDetail> message)
    {
        var exists = await dbContext.ProductDetails
            .AnyAsync(x => x.Id == message.Data.Id);

        if (!exists)
        {
            dbContext.ProductDetails.Add(new ProductDetail()
            {
                CompanyId =  message.Data.CompanyId,
                ProductId =  message.Data.ProductId,
                Id =  message.Data.Id,
                Price =  message.Data.Price,
                Name =  message.Data.Name,
                ProductDimensions = message.Data.ProductDimensions,
                ImageToShow =  message.Data.ImageToShow,
                Sku =  message.Data.Sku,
                WeightKg =  message.Data.WeightKg,
                IsDeleted = false,
                Version = 0,
                
            });

            await dbContext.SaveChangesAsync();
        }
    }
}