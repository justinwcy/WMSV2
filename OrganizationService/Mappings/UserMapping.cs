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

        public static Staff ToModel(this StaffRegisterDTO staffRegisterDTO)
        {
            return new Staff()
            {
                UserName = staffRegisterDTO.Username,
                Email = staffRegisterDTO.Email,
                Password = staffRegisterDTO.Password,
                CompanyId = staffRegisterDTO.CompanyId
            };
        }

        public static Staff ToModel(this StaffUpdateDTO staffUpdateDTO)
        {
            return new Staff()
            {
                UserName = staffUpdateDTO.Username,
                Email = staffUpdateDTO.Email,
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
