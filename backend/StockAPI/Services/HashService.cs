using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace StockAPI.Services
{
    public class HashService
    {
        private readonly ILogger<HashService> _logger;

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
        }

        public string ComputeSha256Hash(string input)
        {
            _logger.LogInformation("Hash hesaplanıyor - Input: {Input}", input);
            
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                var result = Convert.ToBase64String(bytes);
                _logger.LogInformation("Hash sonucu - Result: {Result}", result);
                return result;
            }
        }
    }
} 