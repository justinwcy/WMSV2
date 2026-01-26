using Microsoft.AspNetCore.Identity;
using OrganizationService.DTOs;
using OrganizationService.Mappings;
using OrganizationService.Models;

namespace OrganizationService.Service
{
    public class CompanyService(
        UserManager<Staff> userManager) : ICompanyService
    {
        public async Task<CompanyReadDTO> GetCompanyWithUserAndRoles(Company company)
        {
            CompanyReadDTO companyReadDTO = company.ToReadDTO();
            List<StaffReadDTO> staffReadDTOs = new List<StaffReadDTO>();
            foreach (Staff staff in company.Staffs)
            {
                IEnumerable<string> roles = await userManager.GetRolesAsync(staff);
                StaffReadDTO staffReadDTO = staff.ToReadDTO(roles);
                staffReadDTOs.Add(staffReadDTO);
            }

            companyReadDTO.Staffs = staffReadDTOs;
            return companyReadDTO;
        }

        public async Task<IEnumerable<CompanyReadDTO>> GetCompanyWithUserAndRoles(IEnumerable<Company> companies)
        {
            List<CompanyReadDTO> companyReadDTOs = new List<CompanyReadDTO>();
            foreach (Company company in companies)
            {
                CompanyReadDTO companyReadDTO = await GetCompanyWithUserAndRoles(company);
                companyReadDTOs.Add(companyReadDTO);
            }
            return companyReadDTOs;
        }
    }
}
