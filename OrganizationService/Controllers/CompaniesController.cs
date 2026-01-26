using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizationService.Constants;
using OrganizationService.DTOs;
using OrganizationService.Mappings;
using OrganizationService.Models;
using OrganizationService.Repositories;
using OrganizationService.Service;
using WMSCommon.Results;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/v1/OrganizationService/[controller]")]
    public class CompaniesController(
        ICompanyRepository companyRepository,
        ICompanyService companyService) : ControllerBase
    {
        [Authorize]
        [HttpGet("{id:guid}", Name = "GetCompanyById")]
        public async Task<ActionResult<CompanyReadDTO>> GetCompanyById(Guid id)
        {
            var company = await companyRepository.GetByIdAsync(
                id,
                include: q => q.Include(c => c.Staffs));
            if (company == null)
            {
                return NotFound();
            }

            CompanyReadDTO companyReadDTO = await companyService.GetCompanyWithUserAndRoles(company);
            return Ok(companyReadDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CompanyReadDTO>> Create(CompanyCreateDTO companyCreateDTO)
        {
            var company = companyCreateDTO.ToModel();
            var result = await companyRepository.CreateAsync(company);
            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return CreatedAtRoute(nameof(GetCompanyById),
                new { Id = company.Id }, company.ToReadDTO());
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

            CompanyReadDTO companyReadDTO = await companyService.GetCompanyWithUserAndRoles(updateCompanyResult.Data);
            return Ok(companyReadDTO);
        }

        [Authorize(Roles = nameof(Role.MasterControl))]
        [HttpGet]
        public async Task<ActionResult<PaginationResult<CompanyReadDTO>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var companies = await companyRepository.GetAsync(
                pageNumber, 
                pageSize,
                orderBy: c => c.Name,
                include: q => q.Include(c => c.Staffs)
                );
            int companyCount = await companyRepository.CountAsync();

            IEnumerable<CompanyReadDTO> companyReadDTOs = await companyService
                .GetCompanyWithUserAndRoles(companies);
            
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
