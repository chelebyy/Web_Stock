using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByGroups
{
    public class GetPermissionsByGroupsQuery : IRequest<IEnumerable<PermissionGroupDto>>
    {
    }
} 