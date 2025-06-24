using MediatR;
using Stock.Domain.Common; // Result için

namespace Stock.Application.Features.Permissions.Commands
{
    // İzin güncelleme komutu
    // İşlemin başarılı olup olmadığını Result ile döner
    public record UpdatePermissionCommand(
        int Id, 
        string Name, 
        string Description, 
        string ResourceType, 
        string ResourceName, 
        string Action, 
        string Group) : IRequest<Result>;
} 