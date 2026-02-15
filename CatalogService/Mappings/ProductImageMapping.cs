using CatalogService.DTOs;
using CatalogService.Models;

namespace CatalogService.Mappings
{
    public static class ProductImageMapping
    {
        public static ProductImage ToModel(this ProductImageCreateDTO productImageCreateDTO)
        {
            return new ProductImage()
            {
                ImageBase64 = productImageCreateDTO.ImageBase64,
                ProductId = productImageCreateDTO.ProductId,
            };
        }

        public static ProductImage ToModel(this ProductImageUpdateDTO productImageUpdateDTO)
        {
            return new ProductImage()
            {
                ImageBase64 = productImageUpdateDTO.ImageBase64,
                ProductId = productImageUpdateDTO.ProductId,
            };
        }

        public static ProductImageReadDTO ToReadDTO(this ProductImage productImage)
        {
            return new ProductImageReadDTO()
            {
                Id = productImage.Id,
                ImageBase64 = productImage.ImageBase64,
                ProductId = productImage.ProductId,
                CompanyId = productImage.CompanyId,
            };
        }
    }
}
