using WMSCommon.Contexts;

namespace OrganizationService.Contexts
{
    public class CompanyContext(IHttpContextAccessor httpContextAccessor) : ICompanyContext
    {
        public Guid CompanyId
        {
            get
            {
                var claim = httpContextAccessor.HttpContext?.User.FindFirst("CompanyId")?.Value;

                if (Guid.TryParse(claim, out var companyId))
                {
                    return companyId;
                }

                return Guid.Empty;
            }
        }
    }
}
