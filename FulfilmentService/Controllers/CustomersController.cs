using FulfilmentService.DTOs;
using FulfilmentService.Mappings;
using FulfilmentService.Models;
using FulfilmentService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Results;

namespace FulfilmentService.Controllers
{
    [ApiController]
    [Route("api/v1/FulfilmentService/[controller]")]
    public class CustomersController(
        ICustomerRepository customerRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerReadDTO>> GetCustomerById(Guid id)
        {
            Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> include = q => q
                .Include(c => c.Orders);
            var customer = await customerRepository.GetByIdAsync(
                id,
                include);
            if (customer == null)
            {
                return NotFound();
            }

            CustomerReadDTO customerReadDTO = customer.ToReadDTO();
            return Ok(customerReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerReadDTO>> Create(CustomerCreateDTO customerCreateDTO)
        {
            var customer = customerCreateDTO.ToModel();
            var result = await customerRepository.CreateAsync(customer);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetCustomerById),
                new { Id = customer.Id }, customer.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CustomerReadDTO>> Update(
            Guid id,
            CustomerUpdateDTO customerUpdateDTO)
        {
            Customer customer = customerUpdateDTO.ToModel();
            customer.Id = id;

            RepositoryResult<Customer> updateCustomerResult = 
                await customerRepository.UpdateAsync(customer);
            if (!updateCustomerResult.IsSuccess)
            {
                return StatusCode(500, updateCustomerResult.Message);
            }

            CustomerReadDTO customerReadDTO = updateCustomerResult.Data.ToReadDTO();
            return Ok(customerReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<CustomerReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>> include = q => q
                .Include(c => c.Orders);
            var customers = await customerRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.FirstName,
                include);
            int productCount = await customerRepository.CountAsync();

            IEnumerable<CustomerReadDTO> customerReadDTOs = 
                customers.Select(p => p.ToReadDTO());
            var result = new PaginationResult<CustomerReadDTO>
            {
                Items = customerReadDTOs,
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
            var result = await customerRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
