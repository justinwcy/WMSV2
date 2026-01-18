using Microsoft.AspNetCore.Identity;

namespace OrganizationService.Models
{
    public class StaffRole : IdentityUserRole<Guid>
    {
        public IdentityRole<Guid> Role { get; set; } = null!;
    }
}
