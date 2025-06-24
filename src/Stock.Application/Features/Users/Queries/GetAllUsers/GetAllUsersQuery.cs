using MediatR;
using Stock.Application.Models;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PagedResponse<UserDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
        public string? Name { get; set; }
        public int? RoleId { get; set; }
    }
} 