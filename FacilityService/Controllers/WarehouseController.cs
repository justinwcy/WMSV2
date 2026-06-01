using FacilityService.DTOs;
using FacilityService.Mappings;
using FacilityService.Models;
using FacilityService.Repositories;
using FacilityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Contracts.FacilityService;
using WMSCommon.Results;

namespace FacilityService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/FacilityService/[controller]")]
    public class WarehouseController(
        IWarehouseRepository warehouseRepository,
        IWarehouseService warehouseService) : ControllerBase
    {
        [HttpGet("{id:guid}", Name = "GetWarehouseById")]
        public async Task<ActionResult<WarehouseReadDTO>> GetWarehouseById(Guid id)
        {
            Func<IQueryable<Warehouse>, IIncludableQueryable<Warehouse, object>> include = q => q
                .Include(w => w.Racks)
                .Include(w => w.Staffs);

            var warehouse = await warehouseRepository.GetByIdAsync(
                id,
                include);
            if (warehouse == null)
            {
                return NotFound();
            }

            WarehouseReadDTO warehouseReadDTO = warehouse.ToReadDTO();
            return Ok(warehouseReadDTO);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseReadDTO>> Create(WarehouseCreateDTO warehouseCreateDTO)
        {
            var warehouse = warehouseCreateDTO.ToModel();
            RepositoryResult<Warehouse> result = await warehouseService
                .CreateAndPublishAsync(
                    warehouse,
                    warehouseCreateDTO.RackIds, 
                    warehouseCreateDTO.StaffIds);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetWarehouseById),
                new { Id = warehouse.Id }, warehouse.ToReadDTO());
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<WarehouseReadDTO>> Update(
            Guid id,
            WarehouseUpdateDTO warehouseUpdateDTO)
        {
            Warehouse warehouse = warehouseUpdateDTO.ToModel();
            warehouse.Id = id;

            RepositoryResult<Warehouse> result = await warehouseService
                .UpdateAndPublishAsync(
                    warehouse,
                    warehouseUpdateDTO.RackIds, 
                    warehouseUpdateDTO.StaffIds);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            WarehouseReadDTO warehouseReadDTO = result.Data.ToReadDTO();
            return Ok(warehouseReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<WarehouseReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Warehouse>, IIncludableQueryable<Warehouse, object>> include = q => q
                .Include(w => w.Racks)
                .Include(w => w.Staffs);

            var warehouses = await warehouseRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                include);
            int warehouseCount = await warehouseRepository.CountAsync();

            IEnumerable<WarehouseReadDTO> warehouseReadDTOs = warehouses.Select(p => p.ToReadDTO());
            var result = new PaginationResult<WarehouseReadDTO>
            {
                Items = warehouseReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = warehouseCount
            };

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result = await warehouseService.DeleteAndPublishAsync<WarehouseDeleted<Warehouse>>(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
