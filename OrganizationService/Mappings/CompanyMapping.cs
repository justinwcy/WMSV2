using OrganizationService.DTOs;
using OrganizationService.Models;

namespace OrganizationService.Mappings
{
    public static class CompanyMapping
    {
        public static Company ToModel(this CompanyReadDTO companyReadDTO)
        {
            return new Company()
            {
                Id = companyReadDTO.Id,
                Name = companyReadDTO.Name,
            };
        }

        public static Company ToModel(this CompanyCreateDTO companyCreateDTO)
        {
            return new Company()
            {
                Name = companyCreateDTO.Name,
            };
        }

        public static Company ToModel(this CompanyUpdateDTO companyUpdateDTO)
        {
            return new Company()
            {
                Name = companyUpdateDTO.Name,
            };
        }

        public static CompanyReadDTO ToReadDTO(this Company company)
        {
            return new CompanyReadDTO()
            {
                Id = company.Id, 
                Name = company.Name,
                Staffs = new List<StaffReadDTO>()
            };
        }
    }
}
