using Stock.Application.Common;

namespace Stock.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : ICommand
    {
        public int Id { get; set; }
    }
} 