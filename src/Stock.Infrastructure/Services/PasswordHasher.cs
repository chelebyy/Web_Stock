using Stock.Domain.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace Stock.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly ILogger<PasswordHasher> _logger;

        public PasswordHasher(ILogger<PasswordHasher> logger)
        {
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            try
            {
                _logger.LogInformation("Şifre hash'leniyor...");
                // BCrypt.Net-Next kullanarak şifreyi hashle
                // Varsayılan olarak 13 work factor kullanılıyor
                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
                _logger.LogInformation($"Şifre başarıyla hash'lendi. Hash: {hashedPassword}");
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre hash'leme hatası: {ex.Message}");
                throw;
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                _logger.LogInformation("Şifre doğrulanıyor...");
                _logger.LogInformation($"Girilen şifre uzunluğu: {password?.Length ?? 0}");
                _logger.LogInformation($"Veritabanındaki hash: {passwordHash}");
                
                // BCrypt.Net-Next kullanarak şifreyi doğrula
                var result = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
                _logger.LogInformation($"Şifre doğrulama sonucu: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre doğrulama hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
                // Hata durumunda false döndür
                return false;
            }
        }
    }
} 