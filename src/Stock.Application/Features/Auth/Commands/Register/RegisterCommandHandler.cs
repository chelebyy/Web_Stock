using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Domain.ValueObjects;
using Stock.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<RegisterCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogMessages.LogRegisterAttempt, request.Sicil);

            var existingUserSpec = new UserBySicilSpecification(request.Sicil, includeRole: false);
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpec, cancellationToken);

            if (existingUser != null)
            {
                _logger.LogWarning(LogMessages.LogSicilAlreadyExists, request.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.SicilAlreadyExists
                };
            }

            try
            {
                var passwordHash = _passwordHasher.HashPassword(request.Password);
                _logger.LogInformation(LogMessages.LogPasswordHashedForSicil, request.Sicil, passwordHash);

                // Admin kontrolü için admin rolünü kontrol edelim
                bool isAdmin = false;
                if (request.RoleId.HasValue)
                {
                    var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId.Value, cancellationToken);
                    isAdmin = role != null && role.Name.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                }
                _logger.LogInformation(LogMessages.UserAdminStatusCheck, isAdmin, request.RoleId);

                var fullNameResult = FullName.Create(request.Adi, request.Soyadi);
                if (!fullNameResult.IsSuccess)
                {
                    _logger.LogWarning("FullName creation failed: {Error}", fullNameResult.Error);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = fullNameResult.Error
                    };
                }

                var sicilResult = Sicil.Create(request.Sicil);
                if (!sicilResult.IsSuccess)
                {
                    _logger.LogWarning("Sicil creation failed: {Error}", sicilResult.Error);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = sicilResult.Error
                    };
                }

                var userResult = User.Create(
                    fullNameResult.Value,
                    sicilResult.Value,
                    passwordHash,
                    request.RoleId,
                    isAdmin);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarning(LogMessages.InvalidUserData, userResult.Error);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = userResult.Error
                    };
                }

                var user = userResult.Value;
                await _unitOfWork.Users.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation(LogMessages.LogUserRegisteredWithSicil, request.Sicil, user.Id, user.IsAdmin);

                var registeredUserSpec = new EntityByIdSpecification<User>(user.Id, u => u.Role);
                var registeredUser = await _unitOfWork.Users.FirstOrDefaultAsync(registeredUserSpec, cancellationToken);

                var token = _jwtTokenGenerator.GenerateToken(registeredUser ?? user);
                _logger.LogInformation(LogMessages.LogTokenGeneratedForSicil, request.Sicil ?? "[NULL_SICIL]");

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Adi = user.FullName.Adi,
                    Soyadi = user.FullName.Soyadi,
                    IsAdmin = user.IsAdmin,
                    RoleName = (registeredUser?.Role != null) ? registeredUser.Role.Name.Value : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.LogRegistrationErrorWithSicil, request.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.RegistrationFailed
                };
            }
        }
    }
} 