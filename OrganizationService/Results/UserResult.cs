using OrganizationService.Models;
using WMSCommon.Results;

namespace OrganizationService.Results
{
    public class UserResult
    {
        public IEnumerable<string> Roles { get; set; }
        public bool IsSuccess { get; set; }
        public Staff? User { get; set; }
        public string Message { get; set; } = string.Empty;

        public static UserResult Success(Staff staff, IEnumerable<string> roles) => 
            new() 
            {
                IsSuccess = true, 
                User = staff,
                Roles = roles,
            };
        public static UserResult Failure(string message) => 
            new()
            {
                IsSuccess = false, 
                Message = message
            };
    }
}
