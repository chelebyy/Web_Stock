using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Models;

namespace Stock.Application.Features.Products.Queries.GetProductsSummary
{
    /// <summary>
    /// Ürünlerin özet listesini almak için kullanılan sorgu.
    /// </summary>
    public class GetProductsSummaryQuery : IRequest<PagedResponse<ProductSummaryDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }

        public GetProductsSummaryQuery(int pageNumber, int pageSize, string? search)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Search = search;
        }
    }
} 