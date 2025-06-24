using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Queries
{
    public class GetUserByUsernameQuery : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
    }
} 