namespace FulfilmentService.DTOs;

public class OrderDetailCreateDTO
{
    public Guid OrderId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
}