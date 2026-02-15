using CatalogService.DTOs;
using CatalogService.Mappings;
using CatalogService.Models;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Results;

namespace CatalogService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/CatalogService/[controller]")]
    public class ProductsController(
        IProductRepository productRepository) : ControllerBase
    {
        [HttpGet("{id:guid}", Name = "GetProductById")]
        public async Task<ActionResult<ProductReadDTO>> GetProductById(Guid id)
        {
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = q => q
                .Include(c => c.Images)
                .Include(p => p.Details);

            var product = await productRepository.GetByIdAsync(
                id,
                include);
            if (product == null)
            {
                return NotFound();
            }

            ProductReadDTO productReadDTO = product.ToReadDTO();
            return Ok(productReadDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProductReadDTO>> Create(ProductCreateDTO productCreateDTO)
        {
            var product = productCreateDTO.ToModel();
            var result = await productRepository.CreateAsync(product);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetProductById),
                new { Id = product.Id }, product.ToReadDTO());
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<ProductReadDTO>> Update(
            Guid id,
            ProductUpdateDTO productUpdateDTO)
        {
            Product product = productUpdateDTO.ToModel();
            product.Id = id;

            RepositoryResult<Product> updateProductResult = await productRepository.UpdateAsync(product);
            if (!updateProductResult.IsSuccess)
            {
                return StatusCode(500, updateProductResult.Message);
            }

            ProductReadDTO productReadDTO = updateProductResult.Data.ToReadDTO();
            return Ok(productReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = q => q
                .Include(c => c.Images)
                .Include(p => p.Details);

            var products = await productRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                include);
            int productCount = await productRepository.CountAsync();

            IEnumerable<ProductReadDTO> productReadDTOs = products.Select(p => p.ToReadDTO());
            var result = new PaginationResult<ProductReadDTO>
            {
                Items = productReadDTOs,
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
            var success = await productRepository.DeleteAsync(id);
            if (success)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
