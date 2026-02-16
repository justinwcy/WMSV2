using InboundService.DTOs;
using InboundService.Models;

namespace InboundService.Mappings
{
    public static class VendorMapping
    {
        public static Vendor ToModel(this VendorCreateDTO vendorCreateDTO)
        {
            return new Vendor()
            {
                Name = vendorCreateDTO.Name,
                Email = vendorCreateDTO.Email,
                Address = vendorCreateDTO.Address,
                Notes = vendorCreateDTO.Notes,
            };
        }

        public static Vendor ToModel(this VendorUpdateDTO vendorUpdateDTO)
        {
            return new Vendor()
            {
                Name = vendorUpdateDTO.Name,
                Email = vendorUpdateDTO.Email,
                Address = vendorUpdateDTO.Address,
                Notes = vendorUpdateDTO.Notes,
            };
        }

        public static VendorReadDTO ToReadDTO(this Vendor vendor)
        {
            return new VendorReadDTO()
            {
                Id = vendor.Id,
                CompanyId = vendor.CompanyId,
                Name = vendor.Name,
                Email = vendor.Email,
                Address = vendor.Address,
                Notes = vendor.Notes,
                InboundOrders = vendor.InboundOrders?.Select(io => io.ToReadDTO()) ?? Enumerable.Empty<InboundOrderReadDTO>(),
                
            };
        }
    }
}
