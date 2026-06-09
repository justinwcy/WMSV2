using InboundService.DTOs;
using InboundService.Mappings;
using InboundService.Models;
using InboundService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Results;

namespace InboundService.Controllers
{
    [ApiController]
    [Route("api/v1/InboundService/[controller]")]
    public class InboundOrderDetailsController(
        IInboundOrderDetailRepository inboundOrderDetailRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetInboundOrderDetailById")]
        public async Task<ActionResult<InboundOrderDetailReadDTO>> GetInboundOrderDetailById(Guid id)
        {
            Func<IQueryable<InboundOrderDetail>, IIncludableQueryable<InboundOrderDetail, object>> include = q => q
                .Include(i => i.ProductDetail)
                .Include(i => i.InboundOrder);
            var inboundOrderDetail = await inboundOrderDetailRepository.GetByIdAsync(
                id,
                include);
            if (inboundOrderDetail == null)
            {
                return NotFound();
            }

            InboundOrderDetailReadDTO inboundOrderDetailReadDTO = inboundOrderDetail.ToReadDTO();
            return Ok(inboundOrderDetailReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InboundOrderDetailReadDTO>> Create(InboundOrderDetailCreateDTO inboundOrderDetailCreateDTO)
        {
            var inboundOrderDetail = inboundOrderDetailCreateDTO.ToModel();
            var result = await inboundOrderDetailRepository.CreateAsync(inboundOrderDetail);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetInboundOrderDetailById),
                new { Id = inboundOrderDetail.Id }, inboundOrderDetail.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<InboundOrderDetailReadDTO>> Update(
            Guid id,
            InboundOrderDetailUpdateDTO inboundOrderDetailUpdateDTO)
        {
            InboundOrderDetail inboundOrderDetail = inboundOrderDetailUpdateDTO.ToModel();
            inboundOrderDetail.Id = id;

            RepositoryResult<InboundOrderDetail> updateInboundOrderDetailResult = 
                await inboundOrderDetailRepository.UpdateAsync(inboundOrderDetail);
            if (!updateInboundOrderDetailResult.IsSuccess)
            {
                return StatusCode(500, updateInboundOrderDetailResult.Message);
            }

            InboundOrderDetailReadDTO inboundOrderDetailReadDTO = updateInboundOrderDetailResult.Data.ToReadDTO();
            return Ok(inboundOrderDetailReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<InboundOrderDetailReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<InboundOrderDetail>, IIncludableQueryable<InboundOrderDetail, object>> include = q => q
                .Include(i => i.ProductDetail)
                .Include(i => i.InboundOrder);
            var inboundOrderDetails = await inboundOrderDetailRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Status,
                include);
            int productCount = await inboundOrderDetailRepository.CountAsync();

            IEnumerable<InboundOrderDetailReadDTO> inboundOrderDetailReadDTOs = 
                inboundOrderDetails.Select(p => p.ToReadDTO());
            var result = new PaginationResult<InboundOrderDetailReadDTO>
            {
                Items = inboundOrderDetailReadDTOs,
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
            var result = await inboundOrderDetailRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
