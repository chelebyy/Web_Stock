using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Common; // Result<T> için

namespace Stock.Application.Features.Categories.Commands
{
    // Yeni kategori oluşturma komutu
    // Başarılı olursa oluşturulan CategoryDto'yu döner
    public record CreateCategoryCommand(string Name, string? Description) : IRequest<Result<CategoryDto>>;
} 