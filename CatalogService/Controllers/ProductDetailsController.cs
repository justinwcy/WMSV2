using CatalogService.DTOs;
using CatalogService.Mappings;
using CatalogService.Models;
using CatalogService.Repositories;
using CatalogService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.Results;

namespace CatalogService.Controllers
{
    [ApiController]
    [Route("api/v1/CatalogService/[controller]")]
    public class ProductDetailsController(
        IProductDetailRepository productDetailRepository,
        IProductDetailService productDetailService) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetProductDetailById")]
        public async Task<ActionResult<ProductDetailReadDTO>> GetProductDetailById(Guid id)
        {
            var productDetail = await productDetailRepository.GetByIdAsync(
                id,
                null);
            if (productDetail == null)
            {
                return NotFound();
            }

            ProductDetailReadDTO productDetailReadDTO = productDetail.ToReadDTO();
            return Ok(productDetailReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductDetailReadDTO>> Create(ProductDetailCreateDTO productDetailCreateDTO)
        {
            var productDetail = productDetailCreateDTO.ToModel();
            RepositoryResult<IProductDetail> result = await productDetailService
                .CreateAndPublishAsync<ProductDetailCreated>(productDetail);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetProductDetailById),
                new { Id = productDetail.Id }, productDetail.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ProductDetailReadDTO>> Update(
            Guid id,
            ProductDetailUpdateDTO productDetailUpdateDTO)
        {
            ProductDetail productDetail = productDetailUpdateDTO.ToModel();
            productDetail.Id = id;

            var updateProductDetailResult = 
                await productDetailRepository.UpdateAsync(productDetail);
            if (!updateProductDetailResult.IsSuccess)
            {
                return StatusCode(500, updateProductDetailResult.Message);
            }

            ProductDetailReadDTO productDetailReadDTO = updateProductDetailResult.Data.ToReadDTO();
            return Ok(productDetailReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductDetailReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var productDetails = await productDetailRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                null);
            int productCount = await productDetailRepository.CountAsync();

            IEnumerable<ProductDetailReadDTO> productDetailReadDTOs = 
                productDetails.Select(p => p.ToReadDTO());
            var result = new PaginationResult<ProductDetailReadDTO>
            {
                Items = productDetailReadDTOs,
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
            var result = await productDetailRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
