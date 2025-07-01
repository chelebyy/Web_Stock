using MediatR;
using System.Text.Json.Serialization;

namespace Stock.Application.Features.Permissions.Commands.RemoveUserPermission
{
    public class RemoveUserPermissionCommand : IRequest
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
    }
} 