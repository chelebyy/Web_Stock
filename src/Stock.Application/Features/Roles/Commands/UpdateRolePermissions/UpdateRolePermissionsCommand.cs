using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stock.Application.Features.Roles.Commands.UpdateRolePermissions
{
    /// <summary>
    /// Represents the command to update the permissions for a specific role.
    /// </summary>
    public class UpdateRolePermissionsCommand : IRequest<Unit>
    {
        /// <summary>
        /// The ID of the role to update. This is typically set from the route parameter and not the request body.
        /// </summary>
        [JsonIgnore]
        public int RoleId { get; set; }

        /// <summary>
        /// A list of permission IDs to be assigned to the role. Any existing permissions will be replaced.
        /// </summary>
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
} 