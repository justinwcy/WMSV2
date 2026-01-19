using System.Data;
using System.Linq.Expressions;

using MassTransit;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OrganizationService.Constants;
using OrganizationService.DbContexts;
using OrganizationService.Models;

using WMSCommon.Contexts;
using WMSCommon.Contracts;
using WMSCommon.Results;

namespace OrganizationService.Repositories
{
    public class StaffRepository(
        UserManager<Staff> userManager,
        SignInManager<Staff> signInManager,
        IUserContext companyContext,
        IPublishEndpoint publishEndpoint,
        OrganizationDbContext dbContext) : IStaffRepository
    {
        private Guid CompanyId => companyContext.CompanyId;

        public async Task<Staff?> GetByIdAsync(Guid id)
        {
            Staff? staff = await userManager.Users
                .Where(s=>s.CompanyId == CompanyId)
                .Include(s => s.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(s=>s.Id == id);
            return staff;
        }

        public async Task<IReadOnlyList<Staff>> GetAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<Staff, object>>? orderBy = null, 
            bool descending = false)
        {
            int skip = (Math.Max(1, pageNumber) - 1) * pageSize;

            IQueryable<Staff> query = userManager.Users
                .AsNoTracking()
                .Where(s => s.CompanyId == CompanyId)
                .Include(s => s.UserRoles)
                .ThenInclude(ur => ur.Role);

            if (orderBy != null)
            {
                query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderBy(s => s.Email);
            }

            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<bool> AddRoles(Guid staffId, IEnumerable<string> roles)
        {
            IEnumerable<string> validRoles = GetAllRoles();
            foreach (var role in roles)
            {
                var roleValid = validRoles.Any(r => r == role);
                if (!roleValid)
                {
                    return false;
                }
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            Staff? staffFound = await GetByIdAsync(staffId);
            if (staffFound == null)
            {
                return false;
            }

            // add roles
            var roleResult = await userManager.AddToRolesAsync(staffFound, roles);
            return roleResult.Succeeded;
        }

        public async Task<RepositoryResult<Staff>> CreateAsync(Staff entity)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var staffFound = await userManager.FindByIdAsync(entity.Id.ToString());
                if (staffFound != null)
                {
                    return RepositoryResult<Staff>.Failure("User not found");
                }

                var identityResult = await userManager.CreateAsync(entity, entity.Password);
                if (!identityResult.Succeeded)
                {
                    return RepositoryResult<Staff>.Failure(string.Join(", ", 
                        identityResult.Errors.Select(x => x.Description)));
                }
                
                // publish event into outbox
                var staffCreated = new StaffCreated() { Id = entity.Id };
                await publishEndpoint.Publish(staffCreated);

                // commit the whole changes in 1 transaction
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return RepositoryResult<Staff>.Success(entity);
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                return RepositoryResult<Staff>.Failure(exception.Message);
            }
        }

        public async Task<RepositoryResult<Staff>> UpdateAsync(Staff entity)
        {
            var staff = await userManager.FindByIdAsync(entity.Id.ToString());
            if (staff == null)
            {
                return RepositoryResult<Staff>.Failure("User not found");
            }

            staff.Email = entity.Email;
            staff.UserName = entity.UserName;

            var result = await userManager.UpdateAsync(staff);
            if (!result.Succeeded)
            {
                RepositoryResult<Staff> repositoryResult = RepositoryResult<Staff>.Failure("User not found");
                repositoryResult.Message = string.Join(", ", result.Errors.Select(x => x.Description).ToList());
                return repositoryResult;
            }

            return RepositoryResult<Staff>.Success(entity);
        }

        public async Task<int> CountAsync()
        {
            return await userManager.Users
                .Where(u => u.CompanyId == CompanyId)
                .CountAsync();
        }

        public async Task<RepositoryResult<Staff>> Login(string email, string password)
        {
            Staff? staffFound = await userManager.FindByEmailAsync(email);
            if (staffFound == null)
            {
                return RepositoryResult<Staff>.Failure("Login failed");
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(
                staffFound,
                password,
                false);
            if (!signInResult.Succeeded)
            {
                return RepositoryResult<Staff>.Failure("Login failed");
            }

            RepositoryResult<Staff> result = RepositoryResult<Staff>.Success(staffFound);
            result.Message = "Login success";

            return result;
        }

        public async Task<RepositoryResult<Staff>> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            Staff? staffFound = await userManager.FindByIdAsync(userId);
            if (staffFound == null)
            {
                return RepositoryResult<Staff>.Failure("User not found");
            }

            var result = await userManager.ChangePasswordAsync(
                staffFound,
                oldPassword,
                newPassword
            );

            if (result.Succeeded)
            {
                await userManager.UpdateSecurityStampAsync(staffFound);
                RepositoryResult<Staff> repositoryResult = RepositoryResult<Staff>.Success(staffFound);
                repositoryResult.Message = "Password Changed";

                return repositoryResult;
            }

            return RepositoryResult<Staff>.Failure(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        public IReadOnlyList<string> GetAllRoles()
        {
            return Enum.GetNames<Role>().ToList();
        }

        public async Task<Staff?> GetUserByEmail(string email)
        {
            Staff? staff = await userManager.FindByEmailAsync(email);
            return staff;
        }

        public async Task<IReadOnlyList<string>> GetRoles(Staff staff)
        {
            var roles = await userManager.GetRolesAsync(staff);
            return roles.ToList();
        }

        public async Task<RepositoryResult<Staff>> UpdateUserRoles(Guid id, IEnumerable<string> roles)
        {
            var appUser = await userManager.FindByIdAsync(id.ToString());
            if (appUser == null)
            {
                return RepositoryResult<Staff>.Failure("User not found");
            }

            foreach (var role in roles)
            {
                var roleValid = GetAllRoles().Any(r => r == role);
                if (!roleValid)
                {
                    return RepositoryResult<Staff>.Failure($"Role {role} is not valid");
                }
            }

            var currentUserRoles = await userManager.GetRolesAsync(appUser);
            await userManager.RemoveFromRolesAsync(appUser, currentUserRoles);
            await userManager.AddToRolesAsync(appUser, roles);

            RepositoryResult<Staff> result =  RepositoryResult<Staff>.Success(appUser);
            result.Message = $"User {appUser.UserName} role updated";
            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var staffFound = await userManager.Users
                .Where(s=>s.CompanyId == CompanyId)
                .FirstOrDefaultAsync(s=>s.Id == id);
            if (staffFound == null)
            {
                return false;
            }

            var result = await userManager.DeleteAsync(staffFound);
            var staffDeleted = new StaffDeleted() { Id = id };
            await publishEndpoint.Publish(staffDeleted);

            return result.Succeeded;
        }
    }
}
