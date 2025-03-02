using MediatR;

namespace Stock.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
} 