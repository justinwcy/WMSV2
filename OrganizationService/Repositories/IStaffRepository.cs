using OrganizationService.Models;
using WMSCommon.Repositories;
using WMSCommon.Results;

namespace OrganizationService.Repositories
{
    public interface IStaffRepository : ITenantRepository<Staff>
    {
        public Task<RepositoryResult<Staff>> Login(string email, string password);
        public Task<RepositoryResult<Staff>> ChangePassword(string userId, string oldPassword, string newPassword);
        public IReadOnlyList<string> GetAllRoles();
        public Task<bool> AddRoles(Guid staffId, IEnumerable<string> roles);
        public Task<Staff?> GetUserByEmail(string email);
        public Task<IReadOnlyList<string>> GetRoles(Staff staff);
        public Task<RepositoryResult<Staff>> UpdateUserRoles(Guid id,
            IEnumerable<string> roles);
    }
}
