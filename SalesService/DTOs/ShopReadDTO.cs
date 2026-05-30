namespace SalesService.DTOs;

public class ShopReadDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Website { get; set; }
    public ICollection<Guid> ProductDetailIds { get; set; }
}