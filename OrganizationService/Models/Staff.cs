using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using WMSCommon.Entities;

namespace OrganizationService.Models
{
    public class Staff : IdentityUser<Guid>, ITenantEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserToken> UserTokens { get; private set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public virtual ICollection<IdentityRole<Guid>> UserRoles { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }
}
