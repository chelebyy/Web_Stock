namespace StockAPI.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? SKU { get; set; }
    public string? Barcode { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
