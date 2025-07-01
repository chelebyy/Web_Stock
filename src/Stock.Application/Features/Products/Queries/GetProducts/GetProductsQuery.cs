using MediatR;
using Stock.Application.Models;
using Stock.Application.Features.Products.DTOs;

namespace Stock.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<PagedResponse<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchText { get; set; }
    }
} 