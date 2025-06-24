using MediatR;
using Stock.Application.Features.Categories.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Categories.Queries
{
    // Tüm kategorileri getirmek için sorgu
    // Şimdilik parametre almıyor, ileride filtreleme/sayfalama eklenebilir.
    public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;
} 