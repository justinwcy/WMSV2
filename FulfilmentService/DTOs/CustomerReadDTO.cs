namespace FulfilmentService.DTOs;

public class CustomerReadDTO
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public IEnumerable<OrderReadDTO> Orders { get; set; }
}