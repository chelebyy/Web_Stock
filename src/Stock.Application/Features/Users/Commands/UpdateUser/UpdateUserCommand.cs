using Stock.Application.Common;
using Stock.Application.Models;

namespace Stock.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : ICommand<UserDto>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }
} 