using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Yeni kullanıcı oluşturma komut işleyicisi
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
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
        /// <param name="request">İşlenecek komut</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Oluşturulan kullanıcının ID'si</returns>
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Yeni kullanıcı oluşturuluyor: {request.Username}");

            // Kullanıcı adının benzersiz olduğunu kontrol et
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarn($"Kullanıcı adı zaten mevcut: {request.Username}");
                throw new ApplicationException($"'{request.Username}' kullanıcı adı zaten kullanılıyor.");
            }

            // Şifreyi hashle
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Yeni kullanıcı oluştur
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
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