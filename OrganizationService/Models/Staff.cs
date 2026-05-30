using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using WMSCommon.Contracts.OrganizationService;
using WMSCommon.Entities;

namespace OrganizationService.Models
{
    public class Staff : IdentityUser<Guid>, IStaff
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserToken> UserTokens { get; private set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
    }
}
