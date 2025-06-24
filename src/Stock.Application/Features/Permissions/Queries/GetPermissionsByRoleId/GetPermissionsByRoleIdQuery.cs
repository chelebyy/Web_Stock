using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByRoleId
{
    public class GetPermissionsByRoleIdQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public int RoleId { get; }

        public GetPermissionsByRoleIdQuery(int roleId)
        {
            RoleId = roleId;
        }
    }
} 