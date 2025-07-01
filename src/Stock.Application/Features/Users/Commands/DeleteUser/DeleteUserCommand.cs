using MediatR;

namespace Stock.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        // Constructor'ı kaldırmak veya boş bırakmak genellikle daha iyidir, MediatR doldurur.
        // public DeleteUserCommand(int id)
        // {
        //     Id = id;
        // }
    }
} 