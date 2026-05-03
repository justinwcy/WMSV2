using CatalogService.DTOs;
using CatalogService.Models;
using WMSCommon.Contracts.CatalogService;

namespace CatalogService.Mappings
{
    public static class ProductDetailMapping
    {
        public static ProductDetail ToModel(this ProductDetailCreateDTO productDetailCreateDTO)
        {
            return new ProductDetail()
            {
                ImageToShow = productDetailCreateDTO.ImageToShow,
                Name = productDetailCreateDTO.Name,
                Price = productDetailCreateDTO.Price,
                ProductDimensions = productDetailCreateDTO.ProductDimensions.ToModel(),
                Sku = productDetailCreateDTO.Sku,
                WeightKg = productDetailCreateDTO.WeightKg,
                ProductId = productDetailCreateDTO.ProductId,
            };
        }

        public static ProductDetail ToModel(this ProductDetailUpdateDTO productDetailUpdateDTO)
        {
            return new ProductDetail()
            {
                ProductId = productDetailUpdateDTO.ProductId,
                CompanyId = productDetailUpdateDTO.CompanyId,
                ImageToShow = productDetailUpdateDTO.ImageToShow,
                Name = productDetailUpdateDTO.Name,
                Price = productDetailUpdateDTO.Price,
                ProductDimensions = productDetailUpdateDTO.ProductDimensions.ToModel(),
                Sku = productDetailUpdateDTO.Sku,
                WeightKg = productDetailUpdateDTO.WeightKg,
            };
        }

        public static ProductDetailReadDTO ToReadDTO(this IProductDetail productDetail)
        {
            return new ProductDetailReadDTO()
            {
                Id = productDetail.Id,
                ProductId = productDetail.ProductId,
                CompanyId = productDetail.CompanyId,
                ImageToShow = productDetail.ImageToShow,
                Name = productDetail.Name,
                Price = productDetail.Price,
                ProductDimensions = productDetail.ProductDimensions.ToReadDTO(),
                Sku = productDetail.Sku,
                WeightKg = productDetail.WeightKg,
            };
        }
    }
}
