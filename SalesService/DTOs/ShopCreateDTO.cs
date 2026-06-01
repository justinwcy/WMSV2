namespace SalesService.DTOs;

public class ShopCreateDTO
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public ICollection<Guid> ProductDetailIds { get; set; }
}