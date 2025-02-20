using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Roles.Queries.GetRoleById
{
    public class GetRoleByIdQuery : IQuery<RoleWithUsersDto>
    {
        public int Id { get; set; }
    }
} 