using WMSCommon.Entities;

namespace OrganizationService.Models
{
    public class UserToken : TenantEntity
    {
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public bool Invalidated { get; set; }
        public Guid StaffId { get; set; }
        public Staff Staff { get; set; }
    }
}
