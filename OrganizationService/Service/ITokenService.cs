using OrganizationService.Models;

namespace OrganizationService.Service
{
    public interface ITokenService
    {
        public Task<string> CreateToken(Staff staff);

        public Task<string> CreateRefreshToken(Staff staff);

        public Task<Guid> GetUserIdFromRefreshToken(string? refreshToken);

        public Task<bool> DeleteRefreshToken(Guid staffId);
    }
}
