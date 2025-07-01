using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Password { get; set; }
        public string Sicil { get; set; }
        public int? RoleId { get; set; }
    }
} 