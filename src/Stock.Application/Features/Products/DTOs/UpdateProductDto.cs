using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Features.Products.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok seviyesi negatif olamaz.")]
        public int StockLevel { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori ID'si gereklidir.")]
        public int CategoryId { get; set; }
    }
} 