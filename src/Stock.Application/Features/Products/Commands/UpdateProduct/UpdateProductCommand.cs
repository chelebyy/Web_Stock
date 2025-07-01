using MediatR;
using Stock.Domain.Common;

namespace Stock.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result> // Result döndürüyor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StockLevel { get; set; }
        public int CategoryId { get; set; }
    }
} 

