using FulfilmentService.DTOs;
using FulfilmentService.Models;

namespace FulfilmentService.Mappings
{
    public static class OrderMapping
    {
        public static Order ToModel(this OrderCreateDTO orderCreateDTO)
        {
            return new Order()
            {
                Address = orderCreateDTO.Address,
                OrderDetails = [],
                ExpectedArrivalDate =  orderCreateDTO.ExpectedArrivalDate,
                CustomerId =  orderCreateDTO.CustomerId,
                OrderDate =  orderCreateDTO.OrderDate,
            };
        }

        public static Order ToModel(this OrderUpdateDTO orderUpdateDTO)
        {
            return new Order()
            {
                Address = orderUpdateDTO.Address,
                OrderDetails = [],
                ExpectedArrivalDate = orderUpdateDTO.ExpectedArrivalDate,
                CustomerId =  orderUpdateDTO.CustomerId,
                OrderDate = orderUpdateDTO.OrderDate,
            };
        }

        public static OrderReadDTO ToReadDTO(this Order order)
        {
            return new OrderReadDTO()
            {
                Id = order.Id,
                Address =  order.Address,
                OrderDetails = order.OrderDetails?.Select(x=> x.ToReadDTO()) ?? Enumerable.Empty<OrderDetailReadDTO>(),
                ExpectedArrivalDate = order.ExpectedArrivalDate,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
            };
        }
    }
}
