using MediatR;

namespace Stock.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest // Güncelleme işlemi genellikle bir sonuç döndürmez (ya da sadece başarı durumu)
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StockLevel { get; set; }
        public int CategoryId { get; set; }
    }
} 