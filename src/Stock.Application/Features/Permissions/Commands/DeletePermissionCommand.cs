using MediatR;
using Stock.Domain.Common; // Result için

namespace Stock.Application.Features.Permissions.Commands
{
    // İzin silme komutu
    // İşlemin başarılı olup olmadığını Result ile döner
    public record DeletePermissionCommand(int Id) : IRequest<Result>;
} 