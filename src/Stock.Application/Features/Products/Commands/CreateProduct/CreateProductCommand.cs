using MediatR;
using Stock.Application.Features.Products.DTOs;
using System.ComponentModel.DataAnnotations;
using Stock.Domain.Common;

namespace Stock.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<ProductDto>>
    {
        [Required(ErrorMessage = "Ürün adı gereklidir.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok seviyesi negatif olamaz.")]
        public int StockLevel { get; set; }

        [Required(ErrorMessage = "Kategori ID'si gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori ID'si gereklidir.")]
        public int CategoryId { get; set; }
    }
} 