using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.Service
{
    public interface ICompanyService
    {
        public Task<CompanyReadDTO> GetCompanyWithUserAndRoles(Company company);
        public Task<IEnumerable<CompanyReadDTO>> GetCompanyWithUserAndRoles(IEnumerable<Company> companies);
    }
}
