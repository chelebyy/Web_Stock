using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string Sicil { get; set; } = string.Empty;
    }
} 