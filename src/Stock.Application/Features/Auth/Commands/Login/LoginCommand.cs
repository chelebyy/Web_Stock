using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Sicil { get; set; }
        public string Password { get; set; }
    }
} 