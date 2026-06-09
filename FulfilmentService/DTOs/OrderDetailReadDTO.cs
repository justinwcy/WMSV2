namespace FulfilmentService.DTOs;

public class OrderDetailReadDTO
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public OrderReadDTO Order { get; set; }
    public Guid ProductDetailId { get; set; }
    public ProductDetailReadDTO ProductDetail { get; set; }
    public int Quantity { get; set; }
}