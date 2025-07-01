using MediatR;
using Stock.Application.Models.DTOs;
// using Stock.Domain.Common.Result; // Kaldırıldı

namespace Stock.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Sicil { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }
} 