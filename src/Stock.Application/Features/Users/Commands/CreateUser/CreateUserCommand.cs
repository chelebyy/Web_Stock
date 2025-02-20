using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }
} 