using Microsoft.AspNetCore.Identity;

namespace OrganizationService.Models
{
    public class StaffRole : IdentityUserRole<Guid>
    {
        public virtual Staff Staff { get; set; }
        public virtual IdentityRole<Guid> Role { get; set; }
    }
}
