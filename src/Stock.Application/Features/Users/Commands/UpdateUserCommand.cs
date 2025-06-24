using MediatR;
using Stock.Application.Models.DTOs;
// using Stock.Domain.Common.Result; // Kaldırıldı

namespace Stock.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<UserDto?>
    {
        public int Id { get; set; }
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string Sicil { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Password { get; set; }
    }
} 