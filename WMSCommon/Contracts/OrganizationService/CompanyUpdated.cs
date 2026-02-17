namespace WMSCommon.Contracts.OrganizationService
{
    public class CompanyUpdated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
