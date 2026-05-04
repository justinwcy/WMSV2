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
    public class VendorsController(
        IVendorRepository vendorRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetVendorById")]
        public async Task<ActionResult<VendorReadDTO>> GetVendorById(Guid id)
        {
            var vendor = await vendorRepository.GetByIdAsync(
                id,
                null);
            if (vendor == null)
            {
                return NotFound();
            }

            VendorReadDTO vendorReadDTO = vendor.ToReadDTO();
            return Ok(vendorReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VendorReadDTO>> Create(VendorCreateDTO vendorCreateDTO)
        {
            var vendor = vendorCreateDTO.ToModel();
            var result = await vendorRepository.CreateAsync(vendor);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetVendorById),
                new { Id = vendor.Id }, vendor.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<VendorReadDTO>> Update(
            Guid id,
            VendorUpdateDTO vendorUpdateDTO)
        {
            Vendor vendor = vendorUpdateDTO.ToModel();
            vendor.Id = id;

            RepositoryResult<Vendor> updateVendorResult = 
                await vendorRepository.UpdateAsync(vendor);
            if (!updateVendorResult.IsSuccess)
            {
                return StatusCode(500, updateVendorResult.Message);
            }

            VendorReadDTO vendorReadDTO = updateVendorResult.Data.ToReadDTO();
            return Ok(vendorReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<VendorReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var vendors = await vendorRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                null);
            int productCount = await vendorRepository.CountAsync();

            IEnumerable<VendorReadDTO> vendorReadDTOs = 
                vendors.Select(p => p.ToReadDTO());
            var result = new PaginationResult<VendorReadDTO>
            {
                Items = vendorReadDTOs,
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
            var result = await vendorRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
