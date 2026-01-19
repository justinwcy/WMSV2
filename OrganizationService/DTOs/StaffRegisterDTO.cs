using System.ComponentModel.DataAnnotations;

namespace OrganizationService.DTOs
{
    public class StaffRegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public IEnumerable<string> Roles { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
    }
}
