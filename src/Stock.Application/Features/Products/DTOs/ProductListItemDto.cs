namespace Stock.Application.Features.Products.DTOs;

/// <summary>
/// Ürün listelerinde gösterilecek temel ürün bilgilerini içeren DTO.
/// ProductDto'ya göre daha hafiftir ve sadece liste için gerekli verileri taşır.
/// </summary>
public class ProductListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public int StockLevel { get; set; }
} 