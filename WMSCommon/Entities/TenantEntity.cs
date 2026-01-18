namespace WMSCommon.Entities
{
    public class TenantEntity : GenericEntity, ITenantEntity
    {
        public Guid CompanyId { get; set; }
    }
}
