using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrganizationService.DbContexts;
using OrganizationService.Models;
using OrganizationService.Results;
using WMSCommon.Contexts;
using WMSCommon.Contracts.OrganizationService;
using Wolverine;

namespace OrganizationService.Repositories
{
    public class StaffRepository(
        UserManager<Staff> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        SignInManager<Staff> signInManager,
        IUserContext userContext,
        IMessageBus messageBus,
        OrganizationDbContext dbContext) : 
        IStaffRepository
    {
        private Guid CompanyId => userContext.CompanyId;

        public async Task<UserResult> GetByIdAsync(Guid id)
        {
            Staff? staff = await userManager.FindByIdAsync(id.ToString());
            if (staff != null)
            {
                IEnumerable<string> roles = await userManager.GetRolesAsync(staff);
                return UserResult.Success(staff, roles);
            }

            return UserResult.Failure("User not found");
        }

        public async Task<IEnumerable<UserResult>> GetAsync(int pageSize, int pageNumber)
        {
            var staffs = await userManager.Users
                .Where(s=>s.CompanyId == CompanyId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var staffsWithRoles = new List<UserResult>();
            foreach (Staff staff in staffs)
            {
                IEnumerable<string> roles = await userManager.GetRolesAsync(staff);
                staffsWithRoles.Add(UserResult.Success(staff, roles));
            }
            return staffsWithRoles;
        }

        public async Task<int> CountAsync()
        {
            var count = await userManager.Users
                .Where(s=>s.CompanyId == CompanyId)
                .CountAsync();
            return count;
        }

        public async Task<UserResult> LoginAsync(
            string email, 
            string password)
        {
            Staff? staffFound = await userManager.FindByEmailAsync(email);
            if (staffFound == null)
            {
                return UserResult.Failure("Login failed");
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(
                staffFound,
                password,
                false);
            if (!signInResult.Succeeded)
            {
                return UserResult.Failure("Login failed");
            }

            IEnumerable<string> staffRoles = await userManager.GetRolesAsync(staffFound);
            UserResult result = UserResult.Success(staffFound, staffRoles);
            result.Message = "Login success";

            return result;
        }

        public async Task<UserResult> ChangePasswordAsync(
            string userId,
            string oldPassword,
            string newPassword)
        {
            Staff? staffFound = await userManager.FindByIdAsync(userId);
            if (staffFound == null)
            {
                return UserResult.Failure("User not found");
            }

            var result = await userManager.ChangePasswordAsync(
                staffFound,
                oldPassword,
                newPassword
            );

            if (result.Succeeded)
            {
                await userManager.UpdateSecurityStampAsync(staffFound);
                IEnumerable<string> roles = await userManager.GetRolesAsync(staffFound);
                UserResult successResult = UserResult.Success(staffFound, roles);
                successResult.Message = "Password Changed";

                return successResult;
            }

            return UserResult.Failure(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        public async Task<IReadOnlyList<IdentityRole<Guid>>> GetAllRolesAsync()
        {
            var allRoles = await roleManager.Roles.ToListAsync();
            return allRoles;
        }

        public async Task<IReadOnlyList<string>> GetRolesAsync(Staff staff)
        {
            var roles = await userManager.GetRolesAsync(staff);
            return roles.ToList();
        }

        public async Task<UserResult> RegisterAsync(
            Staff staff, 
            string password, 
            IEnumerable<string> roles)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            var createResult = await userManager.CreateAsync(staff, password);
            if (!createResult.Succeeded)
            {
                string message = string.Join("\n", createResult.Errors.Select(e => e.Description));
                return UserResult.Failure(message);
            }

            foreach (var roleName in roles)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await userManager.AddToRoleAsync(staff, roleName);
                    if (!roleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        var errorMessage = string.Join("\n", roleResult.Errors.Select(e => e.Description));
                        return UserResult.Failure(errorMessage);
                    }
                }
            }

            await messageBus.PublishAsync(new StaffCreated<Staff>
            {
                Data = staff,
            });
            
            await transaction.CommitAsync();
            
            var success = UserResult.Success(staff, roles);
            success.Message = "User created successfully";
            return success;
        }

        public async Task<UserResult> UpdateAsync(
            Staff entity,
            IEnumerable<string> roles)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            var staff = await userManager.FindByIdAsync(entity.Id.ToString());
            if (staff == null)
            {
                return UserResult.Failure("User not found");
            }

            staff.Email = entity.Email;
            staff.UserName = entity.UserName;
            staff.FirstName = entity.FirstName;
            staff.LastName = entity.LastName;

            var result = await userManager.UpdateAsync(staff);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                string errorMessage = string.Join(", ", result.Errors.Select(x => x.Description));
                return UserResult.Failure(errorMessage);
            }

            var currentUserRoles = await userManager.GetRolesAsync(staff);
            var removeRolesResult = await userManager.RemoveFromRolesAsync(staff, currentUserRoles);
            if (!removeRolesResult.Succeeded)
            {
                await transaction.RollbackAsync();
                string errorMessage = string.Join(", ", removeRolesResult.Errors.Select(x => x.Description));
                return UserResult.Failure(errorMessage);
            }
            
            var addRolesResult = await userManager.AddToRolesAsync(staff, roles);
            if (!addRolesResult.Succeeded)
            {
                await transaction.RollbackAsync();
                string errorMessage = string.Join(", ", addRolesResult.Errors.Select(x => x.Description));
                return UserResult.Failure(errorMessage);
            }

            staff.Version++;
            var staffUpdated = new StaffUpdated<Staff>
            {
                Data = staff,
            };

            await messageBus.PublishAsync(staffUpdated);
            await transaction.CommitAsync();
            return UserResult.Success(staff, roles);
        }

        public async Task<UserResult> DeleteAsync(Guid id)
        {
            var staffFound = await userManager.Users
                .Where(s => s.CompanyId == CompanyId)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (staffFound == null)
            {
                return UserResult.Failure("User not found");
            }

            var result = await userManager.DeleteAsync(staffFound);
            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(e => e.Description));
                return UserResult.Failure(message);
                
            }

            await messageBus.PublishAsync(new StaffDeleted<Staff>
            {
                Data = staffFound,
            });
            
            // return empty roles because we dont need it
            return UserResult.Success(staffFound, []);
        }
    }
}
