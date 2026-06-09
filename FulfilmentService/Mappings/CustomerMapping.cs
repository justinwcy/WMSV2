using FulfilmentService.DTOs;
using FulfilmentService.Models;

namespace FulfilmentService.Mappings
{
    public static class CustomerMapping
    {
        public static Customer ToModel(this CustomerCreateDTO customerCreateDTO)
        {
            return new Customer()
            {
                FirstName = customerCreateDTO.FirstName,
                LastName = customerCreateDTO.LastName,
                Orders = [],
                Address = customerCreateDTO.Address,
                Email =  customerCreateDTO.Email,
                PhoneNumber = customerCreateDTO.PhoneNumber,
            };
        }

        public static Customer ToModel(this CustomerUpdateDTO customerUpdateDTO)
        {
            return new Customer()
            {
                FirstName = customerUpdateDTO.FirstName,
                LastName = customerUpdateDTO.LastName,
                Orders = [],
                Address = customerUpdateDTO.Address,
                Email =  customerUpdateDTO.Email,
                PhoneNumber = customerUpdateDTO.PhoneNumber,
            };
        }

        public static CustomerReadDTO ToReadDTO(this Customer customer)
        {
            return new CustomerReadDTO()
            {
                Id = customer.Id,
                Address =  customer.Address,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Orders = customer.Orders?.Select(x => x.ToReadDTO()) ?? Enumerable.Empty<OrderReadDTO>(),
                PhoneNumber = customer.PhoneNumber,
            };
        }
    }
}
