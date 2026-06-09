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
    public class OrdersController(
        IOrderRepository orderRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetOrderById")]
        public async Task<ActionResult<OrderReadDTO>> GetOrderById(Guid id)
        {
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = q => q
                .Include(x => x.OrderDetails)
                .Include(x => x.Customer);
            var order = await orderRepository.GetByIdAsync(
                id,
                include);
            if (order == null)
            {
                return NotFound();
            }

            OrderReadDTO orderReadDTO = order.ToReadDTO();
            return Ok(orderReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderReadDTO>> Create(OrderCreateDTO orderCreateDTO)
        {
            var order = orderCreateDTO.ToModel();
            var result = await orderRepository.CreateAsync(order);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetOrderById),
                new { Id = order.Id }, order.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<OrderReadDTO>> Update(
            Guid id,
            OrderUpdateDTO orderUpdateDTO)
        {
            Order order = orderUpdateDTO.ToModel();
            order.Id = id;

            RepositoryResult<Order> updateOrderResult = 
                await orderRepository.UpdateAsync(order);
            if (!updateOrderResult.IsSuccess)
            {
                return StatusCode(500, updateOrderResult.Message);
            }

            OrderReadDTO orderReadDTO = updateOrderResult.Data.ToReadDTO();
            return Ok(orderReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<OrderReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = q => q
                .Include(x => x.OrderDetails)
                .Include(x => x.Customer);
            var orders = await orderRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.ExpectedArrivalDate,
                include);
            int productCount = await orderRepository.CountAsync();

            IEnumerable<OrderReadDTO> orderReadDTOs = 
                orders.Select(p => p.ToReadDTO());
            var result = new PaginationResult<OrderReadDTO>
            {
                Items = orderReadDTOs,
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
            var result = await orderRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
