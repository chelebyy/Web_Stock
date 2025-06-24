using MediatR;
using Stock.Domain.Common; // Result için

namespace Stock.Application.Features.Categories.Commands
{
    // Kategori silme komutu
    // İşlemin başarılı olup olmadığını Result ile döner
    public record DeleteCategoryCommand(int Id) : IRequest<Result>;
} 