using MediatR;

namespace Stock.Application.Features.Permissions.Commands.ResetUserPermissions
{
    public class ResetUserPermissionsCommand : IRequest
    {
        public int UserId { get; set; }

        public ResetUserPermissionsCommand(int userId)
        {
            UserId = userId;
        }
    }
} 