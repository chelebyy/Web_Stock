using MediatR;
using Stock.Application.Features.Products.DTOs;

namespace Stock.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto?> // Ürün bulunamazsa null dönebilir
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
} 