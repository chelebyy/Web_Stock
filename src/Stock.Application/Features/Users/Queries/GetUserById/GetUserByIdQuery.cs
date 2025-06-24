using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public int Id { get; set; }
    }
} 