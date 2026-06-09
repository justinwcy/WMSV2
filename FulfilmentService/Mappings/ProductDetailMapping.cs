using FulfilmentService.DTOs;
using FulfilmentService.Models;

namespace FulfilmentService.Mappings
{
    public static class ProductDetailMapping
    {
        public static ProductDetailReadDTO ToReadDTO(this ProductDetail productDetail)
        {
            return new ProductDetailReadDTO()
            {
                Id = productDetail.Id,
                ProductId = productDetail.ProductId,
                Name = productDetail.Name,
                ImageToShow = productDetail.ImageToShow,
                Price = productDetail.Price,
                ProductDimensions = productDetail.ProductDimensions,
                Sku = productDetail.Sku,
                WeightKg = productDetail.WeightKg,
            };
        }
    }
}
