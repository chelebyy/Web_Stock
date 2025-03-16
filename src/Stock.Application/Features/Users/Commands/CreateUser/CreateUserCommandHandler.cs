using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.Application.Common.CQRS;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Yeni kullanıcı oluşturma komut işleyicisi
    /// </summary>
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir CreateUserCommandHandler örneği oluşturur
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="passwordService">Şifre servisi</param>
        /// <param name="logger">Logger</param>
        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _logger = logger;
        }

        /// <summary>
        /// Komutu işler
        /// </summary>
        /// <param name="command">İşlenecek komut</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Oluşturulan kullanıcının ID'si</returns>
        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Yeni kullanıcı oluşturuluyor: {command.Username}");

            // Kullanıcı adının benzersiz olduğunu kontrol et
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(command.Username);
            if (existingUser != null)
            {
                _logger.LogWarn($"Kullanıcı adı zaten mevcut: {command.Username}");
                throw new ApplicationException($"'{command.Username}' kullanıcı adı zaten kullanılıyor.");
            }

            // Şifreyi hashle
            var passwordHash = _passwordService.HashPassword(command.Password);

            // Yeni kullanıcı oluştur
            var user = new User
            {
                Username = command.Username,
                PasswordHash = passwordHash,
                Email = command.Email,
                FullName = command.FullName,
                RoleId = command.RoleId,
                IsAdmin = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            // Kullanıcıyı veritabanına ekle
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo($"Kullanıcı başarıyla oluşturuldu. ID: {user.Id}");

            return user.Id;
        }
    }
} 