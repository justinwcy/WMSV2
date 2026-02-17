using Microsoft.EntityFrameworkCore;

using OrganizationService.DbContexts;
using OrganizationService.Models;

using WMSCommon.Repositories;
using WMSCommon.Results;

namespace OrganizationService.Repositories
{
    public class CompanyRepository(IDbContextFactory<OrganizationDbContext> organizationDbContextFactory) : 
        GenericRepository<Company, OrganizationDbContext>(organizationDbContextFactory),
        ICompanyRepository
    {
    }
}
