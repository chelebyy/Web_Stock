using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace Stock.Infrastructure.Services
{
    public class HashService : IHashService
    {
        private readonly ILogger<HashService> _logger;
        private const int WORK_FACTOR = 11; // Sabit work factor

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            try
            {
                _logger.LogInformation($"Şifre hash'leniyor... Work Factor: {WORK_FACTOR}");
                
                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, WORK_FACTOR);
                
                _logger.LogInformation($"Şifre başarıyla hash'lendi. Hash: {hashedPassword}");
                _logger.LogInformation($"Hash formatı: {(hashedPassword.StartsWith("$2") ? "BCrypt" : "Bilinmeyen")}");
                _logger.LogInformation($"Hash uzunluğu: {hashedPassword.Length}");
                
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
                _logger.LogInformation($"Hash formatı: {(passwordHash.StartsWith("$2") ? "BCrypt" : "Bilinmeyen")}");
                _logger.LogInformation($"Hash uzunluğu: {passwordHash.Length}");
                
                var result = BCrypt.Net.BCrypt.Verify(password, passwordHash);
                
                _logger.LogInformation($"Şifre doğrulama sonucu: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre doğrulama hatası: {ex.Message}");
                throw;
            }
        }
    }
} 