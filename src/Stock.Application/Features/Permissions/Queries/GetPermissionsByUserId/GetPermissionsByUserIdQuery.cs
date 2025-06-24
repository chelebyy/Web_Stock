using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByUserId
{
    public class GetPermissionsByUserIdQuery : IRequest<IEnumerable<PermissionDto>>
    {
        public int UserId { get; }

        public GetPermissionsByUserIdQuery(int userId)
        {
            UserId = userId;
        }
    }
} 