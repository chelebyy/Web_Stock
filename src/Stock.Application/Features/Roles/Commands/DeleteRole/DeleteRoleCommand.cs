using Stock.Application.Common;

namespace Stock.Application.Features.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommand : ICommand
    {
        public int Id { get; set; }
    }
} 