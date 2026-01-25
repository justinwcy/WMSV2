using Microsoft.AspNetCore.Identity;
using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.Mappings
{
    public static class UserMapping
    {
        public static Staff ToModel(this StaffRegisterDTO staffRegisterDTO)
        {
            ICollection<StaffRole> roles = staffRegisterDTO.RoleIds
                .Select(roleId => new StaffRole
                {
                    RoleId = roleId
                }).ToList();

            return new Staff()
            {
                UserName = staffRegisterDTO.Username,
                FirstName = staffRegisterDTO.FirstName,
                LastName = staffRegisterDTO.LastName,
                Email = staffRegisterDTO.Email,
                Password = staffRegisterDTO.Password,
                CompanyId = staffRegisterDTO.CompanyId,
                UserRoles = roles,
            };
        }

        public static Staff ToModel(this StaffUpdateDTO staffUpdateDTO)
        {
            return new Staff()
            {
                UserName = staffUpdateDTO.Username,
                Email = staffUpdateDTO.Email,
                FirstName = staffUpdateDTO.FirstName,
                LastName = staffUpdateDTO.LastName,
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
                FirstName = staff.FirstName,
                LastName = staff.LastName,
            };
        }
    }
}
