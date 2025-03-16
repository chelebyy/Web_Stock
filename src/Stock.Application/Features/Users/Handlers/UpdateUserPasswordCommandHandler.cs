using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Stock.Application.Features.Users.Handlers
{
    /// <summary>
    /// Kullanıcı şifresini güncellemek için handler
    /// </summary>
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;

        public UpdateUserPasswordCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            ILogger<UpdateUserPasswordCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Kullanıcı şifresi güncelleniyor. Kullanıcı ID: {request.Id}");

            // Kullanıcıyı getir
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning($"ID: {request.Id} olan kullanıcı bulunamadı.");
                return null;
            }

            // Şifreyi hashle
            string passwordHash = BC.HashPassword(request.Password);
            
            // Şifreyi güncelle
            user.PasswordHash = passwordHash;
            
            // Kullanıcıyı güncelle
            await _userRepository.UpdateAsync(user);
            
            _logger.LogInformation($"Kullanıcı şifresi başarıyla güncellendi. Kullanıcı ID: {request.Id}");
            
            return _mapper.Map<UserDto>(user);
        }
    }
} 