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
        public override async Task<RepositoryResult<Company>> UpdateAsync(Company entity)
        {
            await using var dbContext = await ContextFactory.CreateDbContextAsync();
            var existingCompany = await dbContext.Companies
                .Include(c=>c.Staffs)
                .FirstOrDefaultAsync(c => c.Id == entity.Id);

            if (existingCompany == null)
            {
                return RepositoryResult<Company>.Failure("Company not found.");
            }

            existingCompany.Name = entity.Name;
            await dbContext.SaveChangesAsync();

            return RepositoryResult<Company>.Success(existingCompany);
        }
    }
}
