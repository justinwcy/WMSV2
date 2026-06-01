using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SalesService.DTOs;
using SalesService.Mappings;
using SalesService.Models;
using SalesService.Repositories;
using WMSCommon.Results;

namespace SalesService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/SalesService/[controller]")]
    public class ShopController(
        IShopRepository shopRepository) : ControllerBase
    {
        [HttpGet("{id:guid}", Name = "GetShopById")]
        public async Task<ActionResult<ShopReadDTO>> GetShopById(Guid id)
        {
            Func<IQueryable<Shop>, IIncludableQueryable<Shop, object>> include = q => q
                .Include(s => s.ProductDetails);

            var shop = await shopRepository.GetByIdAsync(
                id,
                include);
            if (shop == null)
            {
                return NotFound();
            }

            ShopReadDTO shopReadDTO = shop.ToReadDTO();
            return Ok(shopReadDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ShopReadDTO>> Create(ShopCreateDTO shopCreateDTO)
        {
            var shop = shopCreateDTO.ToModel();
            var result = await shopRepository.CreateAsync(shop);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }
            
            return CreatedAtRoute(nameof(GetShopById),
                new { Id = shop.Id }, shop.ToReadDTO());
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ShopReadDTO>> Update(
            Guid id,
            ShopUpdateDTO shopUpdateDTO)
        {
            Shop shop = shopUpdateDTO.ToModel();
            shop.Id = id;
            var result = await shopRepository.CreateAsync(shop);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            ShopReadDTO shopReadDTO = result.Data.ToReadDTO();
            return Ok(shopReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<ShopReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Shop>, IIncludableQueryable<Shop, object>> include = q => q
                .Include(s => s.ProductDetails);

            var shops = await shopRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                include);
            int shopCount = await shopRepository.CountAsync();

            IEnumerable<ShopReadDTO> shopReadDTOs = shops.Select(p => p.ToReadDTO());
            var result = new PaginationResult<ShopReadDTO>
            {
                Items = shopReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = shopCount
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result = await shopRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
