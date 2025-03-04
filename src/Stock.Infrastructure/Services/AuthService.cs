using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Stock.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            JwtTokenGenerator tokenGenerator,
            ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _logger = logger;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation($"Giriş denemesi: {loginDto.Username}, Şifre uzunluğu: {loginDto.Password?.Length ?? 0}");
            
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
            
            if (user == null)
            {
                _logger.LogWarning($"Kullanıcı bulunamadı: {loginDto.Username}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            _logger.LogInformation($"Kullanıcı bulundu: {loginDto.Username}, ID: {user.Id}, IsAdmin: {user.IsAdmin}");
            _logger.LogInformation($"Şifre doğrulanıyor. Hash: {user.PasswordHash}");
            _logger.LogInformation($"Hash uzunluğu: {user.PasswordHash?.Length ?? 0}");
            _logger.LogInformation($"Hash formatı: {(user.PasswordHash?.StartsWith("$2") == true ? "BCrypt" : "Bilinmeyen")}");
            
            bool isPasswordValid = false;
            try
            {
                // IPasswordHasher arayüzünü kullanarak şifreyi doğrula
                isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
                _logger.LogInformation($"Şifre doğrulama sonucu: {isPasswordValid}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre doğrulama hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Error verifying password"
                };
            }

            if (!isPasswordValid)
            {
                _logger.LogWarning($"Geçersiz şifre: {loginDto.Username}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            var token = _tokenGenerator.GenerateToken(user);
            _logger.LogInformation($"Başarılı giriş: {loginDto.Username}, Token üretildi");

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                RoleName = user.Role?.Name
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogInformation($"Kayıt denemesi: {registerDto.Username}");
            
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
            
            if (existingUser != null)
            {
                _logger.LogWarning($"Kullanıcı adı zaten mevcut: {registerDto.Username}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Username already exists"
                };
            }

            try
            {
                var passwordHash = _passwordHasher.HashPassword(registerDto.Password);
                _logger.LogInformation($"Şifre hash'lendi: {registerDto.Username}, Hash: {passwordHash}");

                var user = new User
                {
                    Username = registerDto.Username,
                    PasswordHash = passwordHash,
                    IsAdmin = false,
                    RoleId = registerDto.RoleId
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Kullanıcı kaydedildi: {registerDto.Username}, ID: {user.Id}");

                var token = _tokenGenerator.GenerateToken(user);
                _logger.LogInformation($"Token üretildi: {registerDto.Username}");

                return new AuthResponseDto
                {
                    Success = true,
                    Token = token,
                    Username = user.Username,
                    IsAdmin = user.IsAdmin
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kayıt hatası: {registerDto.Username}, Hata: {ex.Message}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = $"Error during registration: {ex.Message}"
                };
            }
        }

        public string GenerateJwtToken(UserDto user)
        {
            var userEntity = new User
            {
                Id = user.Id,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                RoleId = user.RoleId
            };

            if (user.RoleId.HasValue && !string.IsNullOrEmpty(user.RoleName))
            {
                userEntity.Role = new Role
                {
                    Id = user.RoleId.Value,
                    Name = user.RoleName
                };
            }

            return _tokenGenerator.GenerateToken(userEntity);
        }

        public async Task<string> GenerateJwtTokenAsync(UserDto user)
        {
            var userEntity = new User
            {
                Id = user.Id,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                RoleId = user.RoleId
            };

            if (user.RoleId.HasValue && !string.IsNullOrEmpty(user.RoleName))
            {
                userEntity.Role = new Role
                {
                    Id = user.RoleId.Value,
                    Name = user.RoleName
                };
            }

            return await _tokenGenerator.GenerateTokenAsync(userEntity);
        }

        public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
        {
            _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
            
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"Kullanıcı bulunamadı - ID: {userId}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Kullanıcı bulunamadı"
                };
            }

            bool isCurrentPasswordValid = false;
            try
            {
                isCurrentPasswordValid = _passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash);
                _logger.LogInformation($"Mevcut şifre doğrulama sonucu: {isCurrentPasswordValid}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre doğrulama hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Şifre doğrulama hatası"
                };
            }

            if (!isCurrentPasswordValid)
            {
                _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Mevcut şifre hatalı"
                };
            }

            try
            {
                user.PasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Şifre başarıyla değiştirildi"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre değiştirme hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = $"Şifre değiştirme hatası: {ex.Message}"
                };
            }
        }
    }
} 