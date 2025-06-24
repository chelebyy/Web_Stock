using MediatR;
using Stock.Application.Models.DTOs;
using Stock.Domain.Common; // Result<T> için

namespace Stock.Application.Features.Permissions.Commands
{
    // Yeni izin oluşturma komutu
    // Başarılı olursa oluşturulan PermissionDto'yu döner
    public record CreatePermissionCommand(
        string Name, 
        string Description, 
        string ResourceType, 
        string ResourceName, 
        string Action, 
        string Group) : IRequest<Result<PermissionDto>>;
} 