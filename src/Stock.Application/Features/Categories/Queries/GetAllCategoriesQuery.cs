using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Application.Models;
using System.Collections.Generic;

namespace Stock.Application.Features.Categories.Queries
{
    // Tüm kategorileri getirmek için sorgu
    // Şimdilik parametre almıyor, ileride filtreleme/sayfalama eklenebilir.
    public class GetAllCategoriesQuery : IRequest<PagedResponse<CategoryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Name { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
    }
} 