namespace Stock.Application.Features.Products.DTOs
{
    /// <summary>
    /// Ürün listelerinde kullanılacak, sadece temel bilgileri içeren hafif bir DTO.
    /// </summary>
    public class ProductSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StockLevel { get; set; }
        public string CategoryName { get; set; }
    }
} 