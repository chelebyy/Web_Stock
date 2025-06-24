using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stock.Application.Features.Users.Commands.UpdateUserPermissions
{
    public class UpdateUserPermissionsCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Key: PermissionId, Value: IsGranted
        /// </summary>
        public Dictionary<int, bool> Permissions { get; set; } = new Dictionary<int, bool>();
    }
} 