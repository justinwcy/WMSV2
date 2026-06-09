using WMSCommon.Contracts;

namespace FulfilmentService.DTOs;

public class ProductDetailReadDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Sku { get; set; }
    public double WeightKg { get; set; }
    public Dimensions ProductDimensions { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ImageToShow { get; set; }
}