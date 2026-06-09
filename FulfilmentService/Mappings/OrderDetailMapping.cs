using FulfilmentService.DTOs;
using FulfilmentService.Models;

namespace FulfilmentService.Mappings
{
    public static class OrderDetailMapping
    {
        public static OrderDetail ToModel(this OrderDetailCreateDTO orderDetailCreateDTO)
        {
            return new OrderDetail()
            {
                OrderId =  orderDetailCreateDTO.OrderId,
                Quantity = orderDetailCreateDTO.Quantity,
                ProductDetailId =  orderDetailCreateDTO.ProductDetailId,
            };
        }

        public static OrderDetail ToModel(this OrderDetailUpdateDTO orderDetailUpdateDTO)
        {
            return new OrderDetail()
            {
                OrderId =  orderDetailUpdateDTO.OrderId,
                Quantity = orderDetailUpdateDTO.Quantity,
                ProductDetailId =  orderDetailUpdateDTO.ProductDetailId,
            };
        }

        public static OrderDetailReadDTO ToReadDTO(this OrderDetail orderDetail)
        {
            return new OrderDetailReadDTO()
            {
                Id = orderDetail.Id,
                OrderId = orderDetail.OrderId,
                Quantity = orderDetail.Quantity,
                ProductDetailId = orderDetail.ProductDetailId,
            };
        }
    }
}
