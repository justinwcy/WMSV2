using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.Mappings
{
    public static class UserMapping
    {
        public static Staff ToModel(this StaffReadDTO userReadDTO)
        {
            return new Staff()
            {
                Id = userReadDTO.Id,
                UserName = userReadDTO.UserName,
                Email = userReadDTO.Email
            };
        }

        public static Staff ToModel(this RegisterDTO registerDTO)
        {
            return new Staff()
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
            };
        }

        public static Staff ToModel(this UpdateStaffDTO updateUserDTO)
        {
            return new Staff()
            {
                UserName = updateUserDTO.Username,
                Email = updateUserDTO.Email,
            };
        }

        public static StaffReadDTO ToReadDTO(this Staff staff)
        {
            return new StaffReadDTO()
            {
                Id = staff.Id,
                UserName = staff.UserName,
                Email = staff.Email,
                Roles = staff.UserRoles.Select(r=>r.Role.Name),
            };
        }
    }
}
