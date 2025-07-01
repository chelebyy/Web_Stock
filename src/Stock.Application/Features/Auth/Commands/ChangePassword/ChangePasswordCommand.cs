using MediatR;
using Stock.Application.Models.DTOs;
using System.Text.Json.Serialization;

namespace Stock.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<AuthResponseDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
} 