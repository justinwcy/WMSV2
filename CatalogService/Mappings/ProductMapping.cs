using CatalogService.DTOs;
using CatalogService.Models;

namespace CatalogService.Mappings
{
    public static class ProductMapping
    {
        public static Product ToModel(this ProductCreateDTO productCreateDTO)
        {
            return new Product()
            {
                Name = productCreateDTO.Name,
                Tags = productCreateDTO.Tags,
                Description = productCreateDTO.Description,
            };
        }

        public static Product ToModel(this ProductUpdateDTO productUpdateDTO)
        {
            return new Product()
            {
                Name = productUpdateDTO.Name,
                Tags = productUpdateDTO.Tags,
                Description = productUpdateDTO.Description,
            };
        }

        public static ProductReadDTO ToReadDTO(this Product product)
        {
            return new ProductReadDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Tags = product.Tags,
                Description = product.Description,
                Details = product.Details.Select(d => d.ToReadDTO()),
                Images = product.Images.Select(i => i.ToReadDTO()),
                CompanyId = product.CompanyId,
            };
        }
    }
}
