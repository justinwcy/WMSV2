using Microsoft.AspNetCore.Identity;

using OrganizationService.Models;
using OrganizationService.Results;
using WMSCommon.Results;

namespace OrganizationService.Repositories
{
    public interface IStaffRepository
    {
        public Task<UserResult> GetByIdAsync(Guid id);

        public Task<IEnumerable<UserResult>> GetAsync(int pageSize, int pageNumber);

        public Task<int> CountAsync();

        public Task<UserResult> LoginAsync(
            string email, 
            string password);

        public Task<UserResult> ChangePasswordAsync(
            string userId, 
            string oldPassword, 
            string newPassword);

        public Task<IReadOnlyList<IdentityRole<Guid>>> GetAllRolesAsync();

        public Task<IReadOnlyList<string>> GetRolesAsync(Staff staff);

        public Task<UserResult> RegisterAsync(
            Staff staff, 
            string password, 
            IEnumerable<string> roles);

        public Task<UserResult> UpdateAsync(
            Staff staff, 
            IEnumerable<string> roles);

        public Task<UserResult> DeleteAsync(Guid id);
    }
}
