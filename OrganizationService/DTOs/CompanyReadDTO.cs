namespace OrganizationService.DTOs
{
    public class CompanyReadDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public IEnumerable<StaffReadDTO> Staffs { get; set; }
    }
}
