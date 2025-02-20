using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : ICommand<RoleDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
} 