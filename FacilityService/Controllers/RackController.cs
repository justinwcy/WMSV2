using FacilityService.Models;
using FacilityService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Results;

namespace FacilityService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/FacilityService/[controller]")]
    public class RackController(
        IRackRepository rackRepository) : ControllerBase
    {
        [HttpGet("{id:guid}", Name = "GetRackById")]
        public async Task<ActionResult<RackReadDTO>> GetRackById(Guid id)
        {
            Func<IQueryable<Rack>, IIncludableQueryable<Rack, object>> include = q => q
                .Include(c => c.Images)
                .Include(p => p.Details);

            var rack = await rackRepository.GetByIdAsync(
                id,
                include);
            if (rack == null)
            {
                return NotFound();
            }

            RackReadDTO rackReadDTO = rack.ToReadDTO();
            return Ok(rackReadDTO);
        }

        [HttpPost]
        public async Task<ActionResult<RackReadDTO>> Create(RackCreateDTO rackCreateDTO)
        {
            var rack = rackCreateDTO.ToModel();
            var result = await rackRepository.CreateAsync(rack);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetRackById),
                new { Id = rack.Id }, rack.ToReadDTO());
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<RackReadDTO>> Update(
            Guid id,
            RackUpdateDTO rackUpdateDTO)
        {
            Rack rack = rackUpdateDTO.ToModel();
            rack.Id = id;

            RepositoryResult<Rack> updateRackResult = await rackRepository.UpdateAsync(rack);
            if (!updateRackResult.IsSuccess)
            {
                return StatusCode(500, updateRackResult.Message);
            }

            RackReadDTO rackReadDTO = updateRackResult.Data.ToReadDTO();
            return Ok(rackReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<RackReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Rack>, IIncludableQueryable<Rack, object>> include = q => q
                .Include(c => c.Images)
                .Include(p => p.Details);

            var racks = await rackRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                include);
            int rackCount = await rackRepository.CountAsync();

            IEnumerable<RackReadDTO> rackReadDTOs = racks.Select(p => p.ToReadDTO());
            var result = new PaginationResult<RackReadDTO>
            {
                Items = rackReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = rackCount
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result = await rackRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
