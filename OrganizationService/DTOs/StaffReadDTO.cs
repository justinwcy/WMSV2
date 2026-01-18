namespace OrganizationService.DTOs
{
    public class StaffReadDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public Guid CompanyId { get; set; }
    }
}
