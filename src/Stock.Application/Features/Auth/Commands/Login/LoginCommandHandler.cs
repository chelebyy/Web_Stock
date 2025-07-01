using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ILogger<LoginCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(LogMessages.LoginAttempt, request.Sicil, request.Password?.Length ?? 0);

                var userSpec = new UserBySicilSpecification(request.Sicil, includeRole: true);
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

                if (user == null)
                {
                    _logger.LogWarning(LogMessages.LogUserNotFoundBySicil, request.Sicil);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = ErrorMessages.InvalidCredentials
                    };
                }

                _logger.LogInformation(LogMessages.UserFound, user.Sicil, user.Id, user.IsAdmin);
                
                bool isPasswordValid = false;
                if (!string.IsNullOrEmpty(user.PasswordHash) && !string.IsNullOrEmpty(request.Password))
                {
                    isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
                }
                _logger.LogInformation(LogMessages.PasswordVerificationResult, isPasswordValid);

                if (!isPasswordValid)
                {
                    _logger.LogWarning(LogMessages.LogInvalidPasswordAttempt, request.Sicil);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = ErrorMessages.InvalidCredentials
                    };
                }

                var token = await _jwtTokenGenerator.GenerateTokenAsync(user);
                _logger.LogInformation(LogMessages.LogLoginSuccess, request.Sicil);

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Adi = user.FullName.Adi,
                    Soyadi = user.FullName.Soyadi,
                    IsAdmin = user.IsAdmin,
                    RoleName = user.Role?.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginCommandHandler'da beklenmedik bir hata oluştu. Sicil: {Sicil}", request.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Giriş işlemi sırasında sunucuda beklenmedik bir hata oluştu. Lütfen sistem yöneticinize başvurun."
                };
            }
        }
    }
} 