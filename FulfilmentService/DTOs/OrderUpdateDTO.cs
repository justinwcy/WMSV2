namespace FulfilmentService.DTOs;

public class OrderUpdateDTO
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedArrivalDate { get; set; }
    public string Address { get; set; }
    public IEnumerable<Guid> OrderDetailIds { get; set; }
}