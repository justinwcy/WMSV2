using CatalogService.DTOs;
using CatalogService.Mappings;
using CatalogService.Models;
using CatalogService.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WMSCommon.Results;

using static MassTransit.ValidationResultExtensions;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/v1/CatalogService/[controller]")]
    public class ProductImagesController(
        IProductImageRepository productImageRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetProductImageById")]
        public async Task<ActionResult<ProductImageReadDTO>> GetProductImageById(Guid id)
        {
            var productImage = await productImageRepository.GetByIdAsync(
                id,
                null);
            if (productImage == null)
            {
                return NotFound();
            }

            ProductImageReadDTO productImageReadDTO = productImage.ToReadDTO();
            return Ok(productImageReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductImageReadDTO>> Create(ProductImageCreateDTO productImageCreateDTO)
        {
            var productImage = productImageCreateDTO.ToModel();
            var result = await productImageRepository.CreateAsync(productImage);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetProductImageById),
                new { Id = productImage.Id }, productImage.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ProductImageReadDTO>> Update(
            Guid id,
            ProductImageUpdateDTO productImageUpdateDTO)
        {
            ProductImage productImage = productImageUpdateDTO.ToModel();
            productImage.Id = id;

            RepositoryResult<ProductImage> updateProductImageResult = 
                await productImageRepository.UpdateAsync(productImage);
            if (!updateProductImageResult.IsSuccess)
            {
                return StatusCode(500, updateProductImageResult.Message);
            }

            ProductImageReadDTO productImageReadDTO = updateProductImageResult.Data.ToReadDTO();
            return Ok(productImageReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var productImages = await productImageRepository.GetAsync(
                pageNumber, 
                pageSize,
                null,
                null);
            int productCount = await productImageRepository.CountAsync();

            IEnumerable<ProductImageReadDTO> productImageReadDTOs = 
                productImages.Select(p => p.ToReadDTO());
            var result = new PaginationResult<ProductImageReadDTO>
            {
                Items = productImageReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = productCount
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result = await productImageRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
