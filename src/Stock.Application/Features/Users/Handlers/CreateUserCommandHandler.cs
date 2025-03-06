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
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IPasswordHasher passwordHasher,
            ILogger<CreateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Kullanıcı oluşturma işlemi başlatıldı: {Username}, Sicil: {Sicil}", request.Username, request.Sicil);
                
                // Girdi parametrelerini kontrol et
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    throw new InvalidOperationException("Kullanıcı adı boş olamaz.");
                }
                
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    throw new InvalidOperationException("Şifre boş olamaz.");
                }
                
                if (string.IsNullOrWhiteSpace(request.Sicil))
                {
                    throw new InvalidOperationException("Sicil numarası boş olamaz.");
                }
                
                // Sicil numarası kontrolü
                _logger.LogInformation("Sicil numarası kontrolü yapılıyor: {Sicil}", request.Sicil);
                var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil);
                if (existingUsers.Any())
                {
                    _logger.LogWarning("Sicil numarası zaten kullanılmakta: {Sicil}", request.Sicil);
                    throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
                }
                
                // IsAdmin değerini sadece RoleId=1 (Admin rolü) olduğunda true yap
                // Bu şekilde frontend'den gönderilen isAdmin değeri görmezden gelinir
                bool isAdmin = request.RoleId.HasValue && request.RoleId.Value == 1;
                
                // Log bilgisi ekleyelim
                _logger.LogInformation("Yeni kullanıcı oluşturuluyor: {Username}, Sicil: {Sicil}, RoleId: {RoleId}, IsAdmin: {IsAdmin}", 
                    request.Username, request.Sicil, request.RoleId, isAdmin);
                
                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = _passwordHasher.HashPassword(request.Password),
                    IsAdmin = isAdmin, // Hesaplanmış isAdmin değerini kullan
                    RoleId = request.RoleId,
                    Sicil = request.Sicil
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var createdUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
                if (createdUser == null)
                {
                    _logger.LogError("Kullanıcı oluşturuldu ancak verilen ID ile bulunamadı: {Id}", user.Id);
                    throw new InvalidOperationException("Kullanıcı oluşturuldu ancak verilen ID ile bulunamadı.");
                }
                
                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu: {Username}, ID: {Id}", createdUser.Username, createdUser.Id);
                return _mapper.Map<UserDto>(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma işlemi sırasında hata oluştu: {ErrorMessage}", ex.Message);
                throw; // Hatayı tekrar fırlat, böylece controller'da yakalanabilir
            }
        }
    }
} 