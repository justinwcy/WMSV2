namespace FulfilmentService.DTOs;

public class OrderDetailUpdateDTO
{
    public Guid OrderId { get; set; }
    public Guid ProductDetailId { get; set; }
    public int Quantity { get; set; }
}