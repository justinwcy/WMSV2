using WMSCommon.Entities;

namespace OrganizationService.Models
{
    public class Company : GenericEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Staff> Staffs { get; set; } = new List<Staff>();
    }
}
