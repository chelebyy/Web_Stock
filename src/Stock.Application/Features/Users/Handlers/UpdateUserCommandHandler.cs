using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user == null)
                return null;

            user.Username = request.Username;
            user.IsAdmin = request.IsAdmin;
            user.RoleId = request.RoleId;
            user.Sicil = request.Sicil;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            return _mapper.Map<UserDto>(updatedUser);
        }
    }
} 