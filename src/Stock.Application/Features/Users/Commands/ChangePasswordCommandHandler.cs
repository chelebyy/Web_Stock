using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Domain.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Application.Constants;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Kullanıcı şifresini değiştirmek için gelen komutu işleyen handler sınıfı.
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILoggerManager<ChangePasswordCommandHandler> _logger;

        /// <summary>
        /// ChangePasswordCommandHandler sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work arayüzü.</param>
        /// <param name="passwordHasher">Şifre hashleme arayüzü.</param>
        /// <param name="logger">Loglama arayüzü.</param>
        public ChangePasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILoggerManager<ChangePasswordCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// ChangePasswordCommand tipindeki isteği işler ve kullanıcı şifresini değiştirir.
        /// </summary>
        /// <param name="request">Şifre değiştirme komutu.</param>
        /// <param name="cancellationToken">İptal jetonu.</param>
        /// <returns>İşlemin başarılı olup olmadığını gösteren boolean değer.</returns>
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo(LogMessages.PasswordChangeRequestForUser, request.UserId);

                // Kullanıcıyı bul
                var userSpec = new EntityByIdSpecification<Stock.Domain.Entities.User>(request.UserId);
                var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

                if (user == null)
                {
                    _logger.LogWarn(LogMessages.UserNotFoundById, request.UserId);
                    throw new NotFoundException($"Kullanıcı bulunamadı: ID {request.UserId}");
                }

                // Mevcut şifreyi doğrula
                bool isCurrentPasswordValid = _passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash);
                if (!isCurrentPasswordValid)
                {
                    _logger.LogWarn(LogMessages.IncorrectOldPassword, request.UserId);
                    throw new ValidationException("Mevcut şifre doğru değil");
                }

                // Yeni şifreyi hashle
                var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);

                // DDD approach: User entity'sinin metodu ile şifreyi değiştir
                var changePasswordResult = user.ChangePassword(newPasswordHash);
                if (!changePasswordResult.IsSuccess)
                {
                    _logger.LogWarn(LogMessages.InvalidUserData, changePasswordResult.Error);
                    throw new ValidationException(changePasswordResult.Error);
                }

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInfo(LogMessages.PasswordChangeSuccessful, request.UserId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ChangePasswordError, request.UserId);
                throw;
            }
        }
    }
} 