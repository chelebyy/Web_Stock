using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stock.Application.Features.Roles.Commands.UpdateRolePermissions
{
    public class UpdateRolePermissionsCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 