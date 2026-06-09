namespace FulfilmentService.DTOs;

public class OrderReadDTO
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ExpectedArrivalDate { get; set; }
    public string Address { get; set; }
    public IEnumerable<OrderDetailReadDTO> OrderDetails { get; set; }
}