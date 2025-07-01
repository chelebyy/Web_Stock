using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Common;

namespace Stock.Application.Features.Auth.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogMessages.LogPasswordChangeAttempt, request.UserId);

            var userSpec = new EntityByIdSpecification<User>(request.UserId, u => u.Role);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning(LogMessages.LogUserNotFoundById, request.UserId);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.UserNotFound
                };
            }

            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarning(LogMessages.LogInvalidCurrentPassword, request.UserId);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.InvalidCurrentPassword
                };
            }

            try
            {
                var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
                var changePasswordResult = user.ChangePassword(newPasswordHash);

                if (!changePasswordResult.IsSuccess)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = changePasswordResult.Error
                    };
                }

                await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(LogMessages.LogPasswordChangedSuccessfully, request.UserId);

                return new AuthResponseDto
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.LogPasswordChangeError, request.UserId);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.PasswordChangeFailed
                };
            }
        }
    }
} 