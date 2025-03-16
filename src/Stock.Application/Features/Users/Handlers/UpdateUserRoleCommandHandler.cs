using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Handlers
{
    /// <summary>
    /// Kullanıcı rolünü güncellemek için handler
    /// </summary>
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserRoleCommandHandler> _logger;

        public UpdateUserRoleCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<UpdateUserRoleCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Kullanıcı rolü güncelleniyor. Kullanıcı ID: {request.Id}, Yeni Rol ID: {request.RoleId}");

            // Kullanıcıyı getir
            var user = await _userRepository.GetByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarning($"ID: {request.Id} olan kullanıcı bulunamadı.");
                return null;
            }

            // Rolü kontrol et
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                _logger.LogWarning($"ID: {request.RoleId} olan rol bulunamadı.");
                return null;
            }

            // Mevcut rolleri temizle ve yeni rolü ekle
            user.Roles.Clear();
            user.Roles.Add(role);
            
            // Kullanıcıyı güncelle
            await _userRepository.UpdateAsync(user);
            
            _logger.LogInformation($"Kullanıcı rolü başarıyla güncellendi. Kullanıcı ID: {request.Id}, Yeni Rol ID: {request.RoleId}");
            
            return _mapper.Map<UserDto>(user);
        }
    }
} 