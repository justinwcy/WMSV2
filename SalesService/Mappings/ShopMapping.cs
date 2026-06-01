using SalesService.DTOs;
using SalesService.Models;

namespace SalesService.Mappings
{
    public static class ShopMapping
    {
        public static Shop ToModel(this ShopReadDTO shopReadDTO)
        {
            return new Shop()
            {
                Id = shopReadDTO.Id,
                Name = shopReadDTO.Name,
                Address = shopReadDTO.Address,
                CompanyId = shopReadDTO.CompanyId,
                ProductDetails = [],
                Website = shopReadDTO.Website,
            };
        }

        public static Shop ToModel(this ShopCreateDTO shopCreateDTO)
        {
            return new Shop()
            {
                Name = shopCreateDTO.Name,
                Address = shopCreateDTO.Address,
                ProductDetails = [],
                Website = shopCreateDTO.Website,
            };
        }

        public static Shop ToModel(this ShopUpdateDTO shopUpdateDTO)
        {
            return new Shop()
            {
                Id = shopUpdateDTO.Id,
                Name = shopUpdateDTO.Name,
                Address = shopUpdateDTO.Address,
                CompanyId = shopUpdateDTO.CompanyId,
                ProductDetails = [],
                Website = shopUpdateDTO.Website,
            };
        }

        public static ShopReadDTO ToReadDTO(this Shop shop)
        {
            return new ShopReadDTO()
            {
                Id = shop.Id, 
                Name = shop.Name,
                Address = shop.Address,
                CompanyId = shop.CompanyId,
                ProductDetailIds = shop.ProductDetails.Select(x => x.Id).ToList(),
                Website = shop.Website,
            };
        }
    }
}