using FulfilmentService.DTOs;
using FulfilmentService.Mappings;
using FulfilmentService.Models;
using FulfilmentService.Repositories;
using FulfilmentService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WMSCommon.Contracts.FulfilmentService;
using WMSCommon.Results;

namespace FulfilmentService.Controllers
{
    [ApiController]
    [Route("api/v1/FulfilmentService/[controller]")]
    public class OrderDetailsController(
        IOrderDetailRepository orderDetailRepository,
        IOrderDetailService orderDetailService) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetOrderDetailById")]
        public async Task<ActionResult<OrderDetailReadDTO>> GetOrderDetailById(Guid id)
        {
            Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>> include = q => q
                .Include(x => x.ProductDetail)
                .Include(x => x.Order);
            var orderDetail = await orderDetailRepository.GetByIdAsync(
                id,
                include);
            if (orderDetail == null)
            {
                return NotFound();
            }

            OrderDetailReadDTO orderDetailReadDTO = orderDetail.ToReadDTO();
            return Ok(orderDetailReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderDetailReadDTO>> Create(OrderDetailCreateDTO orderDetailCreateDTO)
        {
            var orderDetail = orderDetailCreateDTO.ToModel();
            var result = await orderDetailService.CreateAndPublishAsync<OrderDetailCreated<OrderDetail>>(orderDetail);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetOrderDetailById),
                new { Id = orderDetail.Id }, orderDetail.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<OrderDetailReadDTO>> Update(
            Guid id,
            OrderDetailUpdateDTO orderDetailUpdateDTO)
        {
            OrderDetail orderDetail = orderDetailUpdateDTO.ToModel();
            orderDetail.Id = id;
            
            var updateOrderDetailResult = await orderDetailService.UpdateAndPublishAsync<OrderDetailUpdated<OrderDetail>>(orderDetail);
            if (!updateOrderDetailResult.IsSuccess)
            {
                return StatusCode(500, updateOrderDetailResult.Message);
            }

            OrderDetailReadDTO orderDetailReadDTO = updateOrderDetailResult.Data.ToReadDTO();
            return Ok(orderDetailReadDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResult<OrderDetailReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>> include = q => q
                .Include(x => x.ProductDetail)
                .Include(x => x.Order);
            var orderDetails = await orderDetailRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: detail => detail.Quantity,
                include);
            int productCount = await orderDetailRepository.CountAsync();

            IEnumerable<OrderDetailReadDTO> orderDetailReadDTOs = 
                orderDetails.Select(p => p.ToReadDTO());
            var result = new PaginationResult<OrderDetailReadDTO>
            {
                Items = orderDetailReadDTOs,
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
            var result = await orderDetailRepository.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
