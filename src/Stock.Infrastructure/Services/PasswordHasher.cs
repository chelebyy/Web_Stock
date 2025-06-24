using Stock.Domain.Interfaces;
using System;
// using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces; // ILoggerManager için
using Stock.Application.Constants;
using System.Security.Cryptography;
using BCrypt.Net; // BCrypt için using eklendi

namespace Stock.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';
        private const int WORK_FACTOR = 12; // BCrypt için varsayılan work factor

        private readonly ILoggerManager<PasswordHasher> _logger; // Generic tip kullanıldı

        public PasswordHasher(ILoggerManager<PasswordHasher> logger) // Generic tip kullanıldı
        {
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogError(LogMessages.PasswordCannotBeNullOrEmptyForHashing);
                throw new ArgumentNullException(nameof(password), ErrorMessages.PasswordCannotBeNullOrEmpty);
            }

            try
            {
                _logger.LogInfo(LogMessages.PasswordHashingStarted, WORK_FACTOR);

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, WORK_FACTOR);

                _logger.LogInfo(LogMessages.PasswordHashedSuccessfully, hashedPassword);
                LogHashDetails(hashedPassword);

                return hashedPassword;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                _logger.LogError(ex, LogMessages.PasswordHashingSaltError);
                throw new InvalidOperationException(ErrorMessages.PasswordHashingFailed, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.PasswordHashingGenericError);
                throw new InvalidOperationException(ErrorMessages.PasswordHashingFailed, ex);
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordHash))
            {
                _logger.LogError(LogMessages.PasswordOrHashCannotBeNullOrEmptyForVerification);
                // Doğrulama başarısızsa false dön, exception fırlatma
                return false;
            }

            try
            {
                _logger.LogInfo(LogMessages.PasswordVerificationStarted);
                LogHashDetails(passwordHash);

                var result = BCrypt.Net.BCrypt.Verify(password, passwordHash);

                _logger.LogInfo(LogMessages.PasswordVerificationResultLog, result);
                return result;
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                _logger.LogError(ex, LogMessages.PasswordVerificationSaltError);
                // Hata durumunda false dön
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.PasswordVerificationGenericError);
                // Hata durumunda false dön
                return false;
            }
        }

        // Yardımcı metot loglamayı tekrarlamamak için
        private void LogHashDetails(string hash)
        {
            if (string.IsNullOrEmpty(hash)) return;

            _logger.LogDebug(LogMessages.HashFormatLog, (hash.StartsWith("$2") ? HashFormats.BCrypt : HashFormats.Unknown));
            _logger.LogDebug(LogMessages.HashLengthLog, hash.Length);
            try
            {
                // Work factor'u logla (sadece debug seviyesinde)
                var parts = hash.Split('$');
                if (parts.Length > 2 && int.TryParse(parts[2], out int workFactor))
                {
                    _logger.LogDebug(LogMessages.WorkFactorLog, workFactor);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorParsingWorkFactor);
            }
        }
    }

    // Sabitleri buraya veya Constants altına taşıyabiliriz.
    internal static class HashFormats
    {
        public const string BCrypt = "BCrypt";
        public const string Unknown = "Bilinmeyen";
    }
} 