namespace WMSCommon.Entities
{
    public interface ITenantEntity : IGenericEntity
    {
        public Guid CompanyId { get; set; }
    }
}
