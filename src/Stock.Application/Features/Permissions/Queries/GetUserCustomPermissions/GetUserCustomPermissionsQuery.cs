using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Permissions.Queries.GetUserCustomPermissions
{
    public class GetUserCustomPermissionsQuery : IRequest<List<PermissionDto>>
    {
        public int UserId { get; set; }

        public GetUserCustomPermissionsQuery(int userId)
        {
            UserId = userId;
        }
    }
} 