using CatalogService.DTOs;
using CatalogService.Models;

namespace CatalogService.Mappings
{
    public static class ProductDimensionMapping
    {
        public static Dimensions ToModel(this DimensionsDTO dimensionsDTO)
        {
            return new Dimensions(dimensionsDTO.Height, dimensionsDTO.Width, dimensionsDTO.Length);
        }

        public static DimensionsDTO ToReadDTO(this Dimensions dimensions)
        {
            return new DimensionsDTO(dimensions.Height, dimensions.Width, dimensions.Length);
        }
    }
}
