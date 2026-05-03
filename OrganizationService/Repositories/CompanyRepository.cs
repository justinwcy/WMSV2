using OrganizationService.DbContexts;
using OrganizationService.Models;
using WMSCommon.Repositories;

namespace OrganizationService.Repositories
{
    public class CompanyRepository(OrganizationDbContext organizationDbContext) : 
        GenericRepository<Company, OrganizationDbContext>(organizationDbContext),
        ICompanyRepository
    {
    }
}
