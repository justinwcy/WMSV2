using FacilityService.DTOs;
using FacilityService.Models;

namespace FacilityService.Mappings
{
    public static class RackMapping
    {
        public static Rack ToModel(this RackCreateDTO rackCreateDTO)
        {
            return new Rack()
            {
                Name = rackCreateDTO.Name,
                Description = rackCreateDTO.Description,
                DepthMM = rackCreateDTO.DepthMM,
                HeightMM = rackCreateDTO.HeightMM,
                WidthMM = rackCreateDTO.WidthMM,
                WeightKg = rackCreateDTO.WeightKg,
                WarehouseId = rackCreateDTO.WarehouseId,
                Staffs = new List<Staff>(),
            };
        }

        public static Rack ToModel(this RackUpdateDTO rackUpdateDTO)
        {
            return new Rack()
            {
                Name = rackUpdateDTO.Name,
                Description = rackUpdateDTO.Description,
                DepthMM = rackUpdateDTO.DepthMM,
                HeightMM = rackUpdateDTO.HeightMM,
                WidthMM = rackUpdateDTO.WidthMM,
                WeightKg = rackUpdateDTO.WeightKg,
                WarehouseId = rackUpdateDTO.WarehouseId,
            };
        }

        public static RackReadDTO ToReadDTO(this Rack rack)
        {
            return new RackReadDTO()
            {
                Id = rack.Id,
                Name = rack.Name,
                Description = rack.Description,
                DepthMM = rack.DepthMM,
                HeightMM = rack.HeightMM,
                WidthMM = rack.WidthMM,
                WeightKg = rack.WeightKg,
                WarehouseId = rack.WarehouseId,
                StaffIds = rack.Staffs.Select(x => x.Id).ToList(),
                CompanyId = rack.CompanyId,
            };
        }
    }
}