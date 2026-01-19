using System.ComponentModel.DataAnnotations;

namespace OrganizationService.DTOs
{
    public class StaffLoginDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
