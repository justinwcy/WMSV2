namespace WMSCommon.Contexts
{
    public interface IUserContext
    {
        public Guid UserId { get; }
        public Guid CompanyId { get; }
    }
}
