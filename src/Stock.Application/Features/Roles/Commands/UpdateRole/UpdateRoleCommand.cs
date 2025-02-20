using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommand : ICommand<RoleDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 