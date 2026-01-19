using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OrganizationService.Constants;
using OrganizationService.DTOs;
using OrganizationService.Mappings;
using OrganizationService.Models;
using OrganizationService.Repositories;

using WMSCommon.Results;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/v1/OrganizationService/[controller]")]
    public class CompaniesController(
        ICompanyRepository companyRepository) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetById")]
        public async Task<ActionResult<StaffReadDTO>> GetById(Guid id)
        {
            var company = await companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company.ToReadDTO());
        }

        [HttpPost]
        public async Task<ActionResult<CompanyReadDTO>> Create(CompanyCreateDTO companyCreateDTO)
        {
            var company = companyCreateDTO.ToModel();
            var result = await companyRepository.CreateAsync(company);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetById),
                new { userId = company.Id }, company.ToReadDTO());
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<CompanyReadDTO>> Update(
            Guid id,
            CompanyUpdateDTO companyUpdateDTO)
        {
            Company company = companyUpdateDTO.ToModel();
            company.Id = id;

            RepositoryResult<Company> updateCompanyResult = await companyRepository.UpdateAsync(company);
            if (!updateCompanyResult.IsSuccess)
            {
                return StatusCode(500, updateCompanyResult.Message);
            }

            CompanyReadDTO companyReadDTO = updateCompanyResult.Data.ToReadDTO();
            return Ok(companyReadDTO);
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<CompanyReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var companies = await companyRepository.GetAsync(pageSize, pageNumber);
            int companyCount = await companyRepository.CountAsync();
            var companyReadDTOs = new List<CompanyReadDTO>();
            foreach (var staff in companies)
            {
                var userReadDTO = staff.ToReadDTO();
                companyReadDTOs.Add(userReadDTO);
            }

            var result = new PaginationResult<CompanyReadDTO>
            {
                Items = companyReadDTOs,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = companyCount
            };

            return Ok(result);
        }
    }
}
