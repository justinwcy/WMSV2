using FacilityService.DTOs;
using FacilityService.Models;

namespace FacilityService.Mappings
{
    public static class WarehouseMapping
    {
        public static Warehouse ToModel(this WarehouseCreateDTO warehouseCreateDTO)
        {
            return new Warehouse()
            {
                Name = warehouseCreateDTO.Name,
                Address = warehouseCreateDTO.Address,
                Racks = [],
                Staffs = [],
            };
        }

        public static Warehouse ToModel(this WarehouseUpdateDTO warehouseUpdateDTO)
        {
            return new Warehouse()
            {
                Name = warehouseUpdateDTO.Name,
                Address = warehouseUpdateDTO.Address,
                Racks = [],
                Staffs = [],
            };
        }

        public static WarehouseReadDTO ToReadDTO(this Warehouse warehouse)
        {
            return new WarehouseReadDTO()
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Address = warehouse.Address,
                StaffIds = warehouse.Staffs.Select(x => x.Id).ToList(),
                RackIds = warehouse.Racks.Select(x => x.Id).ToList(),
            };
        }
    }
}