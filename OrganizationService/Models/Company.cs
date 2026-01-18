using WMSCommon.Entities;

namespace OrganizationService.Models
{
    public class Company : GenericEntity
    {
        public string Name { get; set; }
        public ICollection<Staff> Staffs { get; set; }
    }
}
