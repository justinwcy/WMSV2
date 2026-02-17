using CatalogService.DTOs;
using WMSCommon.Contracts;

namespace CatalogService.Mappings
{
    public static class ProductDimensionMapping
    {
        public static Dimensions ToModel(this DimensionsDTO dimensionsDTO)
        {
            return new Dimensions(dimensionsDTO.HeightMm, dimensionsDTO.WidthMm, dimensionsDTO.LengthMm);
        }

        public static DimensionsDTO ToReadDTO(this Dimensions dimensions)
        {
            return new DimensionsDTO(dimensions.HeightMm, dimensions.WidthMm, dimensions.LengthMm);
        }
    }
}
