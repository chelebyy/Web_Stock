using MediatR;
using Stock.Domain.Common; // Result için

namespace Stock.Application.Features.Categories.Commands
{
    // Kategori güncelleme komutu
    // İşlemin başarılı olup olmadığını Result ile döner
    public record UpdateCategoryCommand(int Id, string Name, string? Description) : IRequest<Result>;
} 