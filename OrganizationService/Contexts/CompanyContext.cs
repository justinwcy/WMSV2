using System.Security.Claims;

using WMSCommon.Contexts;

namespace OrganizationService.Contexts
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public Guid CompanyId
        {
            get
            {
                var claim = httpContextAccessor.HttpContext?.User.FindFirstValue("CompanyId");

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
                var claim = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(claim, out var companyId))
                {
                    return companyId;
                }

                return Guid.Empty;
            }
        }
    }
}
