using MediatR;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Roles.Queries.GetAllRoles
{
    /// <summary>
    /// Represents the query to retrieve all roles.
    /// </summary>
    public class GetAllRolesQuery : IRequest<PagedResponse<RoleSlimDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public string Name { get; set; }
    }
} 