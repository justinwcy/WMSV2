namespace WMSCommon.Contracts.OrganizationService
{
    public class StaffCreated
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; init; }
        public string UserName { get; init; }
    }
}
