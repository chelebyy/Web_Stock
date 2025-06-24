using MediatR;
using Stock.Application.Features.Categories.DTOs;

namespace Stock.Application.Features.Categories.Queries
{
    // Belirli bir ID'ye sahip kategoriyi getirmek için sorgu
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>; // DTO nullable olabilir, kategori bulunamayabilir
} 