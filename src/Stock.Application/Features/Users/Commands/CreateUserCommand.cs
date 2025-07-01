using MediatR;
using Stock.Application.Models.DTOs;
using Stock.Domain.Common;

namespace Stock.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<Result<UserDto>>
    {
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string Sicil { get; set; } = string.Empty;
    }
} 