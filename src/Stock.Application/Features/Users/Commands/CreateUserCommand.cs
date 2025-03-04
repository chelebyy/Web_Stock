using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string Sicil { get; set; } = string.Empty;
    }
} 