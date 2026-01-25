using Microsoft.AspNetCore.Identity;
using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.Mappings
{
    public static class UserMapping
    {
        public static Staff ToModel(this StaffRegisterDTO staffRegisterDTO)
        {
            return new Staff()
            {
                UserName = staffRegisterDTO.Username,
                FirstName = staffRegisterDTO.FirstName,
                LastName = staffRegisterDTO.LastName,
                Email = staffRegisterDTO.Email,
                CompanyId = staffRegisterDTO.CompanyId,
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

        public static StaffReadDTO ToReadDTO(this Staff staff, IEnumerable<string> roles)
        {
            return new StaffReadDTO()
            {
                Id = staff.Id,
                UserName = staff.UserName,
                Email = staff.Email,
                Roles = roles,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                CompanyId = staff.CompanyId,
            };
        }
    }
}
