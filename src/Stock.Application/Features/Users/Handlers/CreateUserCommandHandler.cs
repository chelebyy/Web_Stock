using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Stock.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Sicil numarası kontrolü
            var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
            }
            
            var user = new User
            {
                Username = request.Username,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                IsAdmin = request.IsAdmin,
                RoleId = request.RoleId,
                Sicil = request.Sicil
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var createdUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            return _mapper.Map<UserDto>(createdUser);
        }
    }
} 