using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user == null)
                return false;

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            return true;
        }
    }
} 