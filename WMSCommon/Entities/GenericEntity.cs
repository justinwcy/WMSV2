namespace WMSCommon.Entities
{
    public abstract class GenericEntity : IGenericEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
    }
}
