using InboundService.DTOs;
using InboundService.Mappings;
using InboundService.Models;
using InboundService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMSCommon.Results;

namespace InboundService.Controllers
{
    [ApiController]
    [Route("api/v1/InboundService/[controller]")]
    public class InboundOrdersController(
        IInboundOrderRepository inboundOrderRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetInboundOrderById")]
        public async Task<ActionResult<InboundOrderReadDTO>> GetInboundOrderById(Guid id)
        {
            var inboundOrder = await inboundOrderRepository.GetByIdAsync(
                id,
                null);
            if (inboundOrder == null)
            {
                return NotFound();
            }

            InboundOrderReadDTO inboundOrderReadDTO = inboundOrder.ToReadDTO();
            return Ok(inboundOrderReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InboundOrderReadDTO>> Create(InboundOrderCreateDTO inboundOrderCreateDTO)
        {
            var inboundOrder = inboundOrderCreateDTO.ToModel();
            var result = await inboundOrderRepository.CreateAsync(inboundOrder);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetInboundOrderById),
                new { Id = inboundOrder.Id }, inboundOrder.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<InboundOrderReadDTO>> Update(
            Guid id,
            InboundOrderUpdateDTO inboundOrderUpdateDTO)
        {
            InboundOrder inboundOrder = inboundOrderUpdateDTO.ToModel();
            inboundOrder.Id = id;

            RepositoryResult<InboundOrder> updateInboundOrderResult = 
                await inboundOrderRepository.UpdateAsync(inboundOrder);
            if (!updateInboundOrderResult.IsSuccess)
            {
                return StatusCode(500, updateInboundOrderResult.Message);
            }

            InboundOrderReadDTO inboundOrderReadDTO = updateInboundOrderResult.Data.ToReadDTO();
            return Ok(inboundOrderReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<InboundOrderReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var inboundOrders = await inboundOrderRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                null);
            int productCount = await inboundOrderRepository.CountAsync();

            IEnumerable<InboundOrderReadDTO> inboundOrderReadDTOs = 
                inboundOrders.Select(p => p.ToReadDTO());
            var result = new PaginationResult<InboundOrderReadDTO>
            {
                Items = inboundOrderReadDTOs,
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
            var success = await inboundOrderRepository.DeleteAsync(id);
            if (success)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
