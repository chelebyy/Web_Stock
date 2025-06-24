using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading.Tasks;
using System;
using Stock.Application.Constants;
using Stock.Domain.Specifications;
// using Stock.Application.Models.Auth; // Yorum satırı yapıldı (CS0234)

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// Kullanıcı kimlik doğrulama, kayıt ve şifre yönetimi işlemlerini gerçekleştiren servis.
    /// IAuthService arayüzünü implemente eder.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILoggerManager<AuthService> _logger;

        /// <summary>
        /// AuthService sınıfının yapıcı metodu.
        /// Gerekli bağımlılıkları (IUnitOfWork, IPasswordHasher, JwtTokenGenerator, ILoggerManager) enjekte eder.
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemleri için Unit of Work arayüzü.</param>
        /// <param name="passwordHasher">Şifre hashleme ve doğrulama işlemleri için arayüz.</param>
        /// <param name="tokenGenerator">JWT token üretimi için sınıf.</param>
        /// <param name="logger">Loglama işlemleri için ILoggerManager arayüzü.</param>
        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            ILoggerManager<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri (sicil numarası ve şifre).</param>
        /// <returns>
        /// Başarılı girişte JWT token ve kullanıcı bilgilerini içeren bir AuthResponseDto.
        /// Başarısız girişte Success=false ve ilgili hata mesajını içeren bir AuthResponseDto.
        /// </returns>
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInfo(LogMessages.LoginAttempt, loginDto.Sicil, loginDto.Password?.Length ?? 0);
            
            var userSpec = new UserBySicilSpecification(loginDto.Sicil, includeRole: true);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec);
            
            if (user == null)
            {
                _logger.LogWarn(LogMessages.LogUserNotFoundBySicil, loginDto.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.InvalidCredentials
                };
            }

            _logger.LogInfo(LogMessages.UserFound, user.Sicil, user.Id, user.IsAdmin);
            _logger.LogInfo(LogMessages.PasswordVerificationStarting, user.PasswordHash);
            _logger.LogInfo(LogMessages.HashLength, user.PasswordHash?.Length ?? 0);
            _logger.LogInfo(LogMessages.HashFormat, (user.PasswordHash?.StartsWith("$2") == true ? "BCrypt" : "Bilinmeyen"));
            
            bool isPasswordValid = false;
            try
            {
                if (user.PasswordHash != null && loginDto.Password != null)
                {
                    isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
                }
                _logger.LogInfo(LogMessages.PasswordVerificationResult, isPasswordValid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.PasswordVerificationError);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.PasswordVerificationFailed
                };
            }

            if (!isPasswordValid)
            {
                _logger.LogWarn(LogMessages.LogInvalidPasswordAttempt, loginDto.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.InvalidCredentials
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            _logger.LogInfo(LogMessages.LogLoginSuccess, loginDto.Sicil);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Adi = user.Adi,
                Soyadi = user.Soyadi,
                IsAdmin = user.IsAdmin,
                RoleName = user.Role?.Name
            };
        }

        /// <summary>
        /// Yeni kullanıcı kaydı yapar.
        /// </summary>
        /// <param name="registerDto">Kayıt bilgileri (kullanıcı adı, şifre, rol ID).</param>
        /// <returns>
        /// Başarılı kayıtta JWT token ve kullanıcı bilgilerini içeren bir AuthResponseDto.
        /// Başarısız kayıtta Success=false ve ilgili hata mesajını içeren bir AuthResponseDto.
        /// </returns>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogInfo(LogMessages.LogRegisterAttempt, registerDto.Sicil);
            
            var existingUserSpec = new UserBySicilSpecification(registerDto.Sicil, includeRole: false);
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpec);
            
            if (existingUser != null)
            {
                _logger.LogWarn(LogMessages.LogSicilAlreadyExists, registerDto.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.SicilAlreadyExists
                };
            }

            try
            {
                var passwordHash = _passwordHasher.HashPassword(registerDto.Password);
                _logger.LogInfo(LogMessages.LogPasswordHashedForSicil, registerDto.Sicil, passwordHash);

                bool isAdmin = registerDto.RoleId.HasValue && registerDto.RoleId.Value == 1;
                _logger.LogInfo(LogMessages.UserAdminStatusCheck, isAdmin, registerDto.RoleId);

                // DDD approach: Use factory method to create User entity
                var userResult = User.Create(
                    registerDto.Adi,
                    registerDto.Soyadi,
                    registerDto.Sicil,
                    passwordHash,
                    registerDto.RoleId,
                    isAdmin);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarn(LogMessages.InvalidUserData, userResult.Error);
                    return new AuthResponseDto
                    {
                        Success = false,
                        ErrorMessage = userResult.Error
                    };
                }

                var user = userResult.Value;
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInfo(LogMessages.LogUserRegisteredWithSicil, registerDto.Sicil, user.Id, user.IsAdmin);

                var registeredUserSpec = new EntityByIdSpecification<User>(user.Id, u => u.Role);
                var registeredUser = await _unitOfWork.Users.FirstOrDefaultAsync(registeredUserSpec);

                var token = _jwtTokenGenerator.GenerateToken(registeredUser ?? user);
                _logger.LogInfo(LogMessages.LogTokenGeneratedForSicil, registerDto.Sicil ?? "[NULL_SICIL]");

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Adi = user.Adi,
                    Soyadi = user.Soyadi,
                    IsAdmin = user.IsAdmin,
                    RoleName = (registeredUser?.Role != null) ? registeredUser.Role.Name : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.LogRegistrationErrorWithSicil, registerDto.Sicil);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.RegistrationFailed
                };
            }
        }

        /// <summary>
        /// Verilen UserDto nesnesi için JWT token üretir (Senkron versiyon).
        /// </summary>
        /// <param name="user">JWT token üretilecek kullanıcı bilgileri.</param>
        /// <returns>Üretilen JWT token string'i.</returns>
        public string GenerateJwtToken(UserDto user)
        {
            // DDD approach: Use factory method and set up a temporary user for token generation
            var userResult = User.Create(
                user.Adi,
                user.Soyadi,
                user.Sicil,
                "temporary-no-password", // Temporary password hash - not used for authentication
                user.RoleId,
                user.IsAdmin);

            if (!userResult.IsSuccess)
            {
                throw new InvalidOperationException(userResult.Error);
            }

            var userEntity = userResult.Value;
            // Override the Id since this is a reconstituted entity
            typeof(BaseEntity).GetProperty("Id").SetValue(userEntity, user.Id);

            if (user.RoleId.HasValue && !string.IsNullOrEmpty(user.RoleName))
            {
                // We need to set up the Role for JWT claims
                var roleResult = Role.Create(user.RoleName, string.Empty);
                if (roleResult.IsSuccess)
                {
                    var role = roleResult.Value;
                    // Override the Id since this is a reconstituted entity
                    typeof(BaseEntity).GetProperty("Id").SetValue(role, user.RoleId.Value);
                    userEntity.AssignRole(user.RoleId.Value, role);
                }
            }

            return _jwtTokenGenerator.GenerateToken(userEntity);
        }

        /// <summary>
        /// Verilen UserDto nesnesi için JWT token üretir (Asenkron versiyon).
        /// </summary>
        /// <param name="user">JWT token üretilecek kullanıcı bilgileri.</param>
        /// <returns>Üretilen JWT token string'ini içeren bir Task.</returns>
        public async Task<string> GenerateJwtTokenAsync(UserDto user)
        {
            // DDD approach: Use factory method and set up a temporary user for token generation
            var userResult = User.Create(
                user.Adi,
                user.Soyadi,
                user.Sicil,
                "temporary-no-password", // Temporary password hash - not used for authentication
                user.RoleId,
                user.IsAdmin);

            if (!userResult.IsSuccess)
            {
                throw new InvalidOperationException(userResult.Error);
            }

            var userEntity = userResult.Value;
            // Override the Id since this is a reconstituted entity
            typeof(BaseEntity).GetProperty("Id").SetValue(userEntity, user.Id);

            if (user.RoleId.HasValue && !string.IsNullOrEmpty(user.RoleName))
            {
                // We need to set up the Role for JWT claims
                var roleResult = Role.Create(user.RoleName, string.Empty);
                if (roleResult.IsSuccess)
                {
                    var role = roleResult.Value;
                    // Override the Id since this is a reconstituted entity
                    typeof(BaseEntity).GetProperty("Id").SetValue(role, user.RoleId.Value);
                    userEntity.AssignRole(user.RoleId.Value, role);
                }
            }

            return await _jwtTokenGenerator.GenerateTokenAsync(userEntity);
        }

        /// <summary>
        /// Belirtilen kullanıcının şifresini değiştirir.
        /// </summary>
        /// <param name="changePasswordDto">Eski şifre ve yeni şifre bilgilerini içeren DTO.</param>
        /// <param name="userId">Şifresi değiştirilecek kullanıcının ID'si.</param>
        /// <returns>
        /// Başarılı olursa Success=true içeren bir AuthResponseDto.
        /// Başarısız olursa Success=false ve ilgili hata mesajını içeren bir AuthResponseDto.
        /// </returns>
        public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
        {
            _logger.LogInfo(LogMessages.ChangePasswordAttempt, userId);
            
            var spec = new EntityByIdSpecification<User>(userId);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
            if (user == null)
            {
                _logger.LogWarn(LogMessages.UserNotFoundById, userId);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.UserNotFound
                };
            }

            if (!_passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarn(LogMessages.IncorrectOldPassword, userId);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = ErrorMessages.IncorrectPassword
                };
            }
            
            var newPasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
            
            // DDD yaklaşımı: User entity'sinin metodu ile şifre değiştirme
            var passwordResult = user.ChangePassword(newPasswordHash);
            if (!passwordResult.IsSuccess)
            {
                _logger.LogWarn(LogMessages.InvalidUserData, passwordResult.Error);
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = passwordResult.Error
                };
            }

            user.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInfo(LogMessages.PasswordChangedSuccessfully, userId);
            return new AuthResponseDto { Success = true };
        }
    }
} 