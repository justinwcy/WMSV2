using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using WMSCommon.Constants;

namespace WMSCommon.Contexts
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public Guid CompanyId
        {
            get
            {
                var claim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimKey.CompanyIdKey);

                if (Guid.TryParse(claim, out var companyId))
                {
                    return companyId;
                }

                return Guid.Empty;
            }
        }

        public Guid UserId
        {
            get
            {
                var claim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimKey.UserIdKey);

                if (Guid.TryParse(claim, out var userId))
                {
                    return userId;
                }

                return Guid.Empty;
            }
        }
    }
}
