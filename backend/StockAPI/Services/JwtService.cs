using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockAPI.Models;
using Microsoft.Extensions.Logging;

namespace StockAPI.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret ?? "");
            var tokenHandler = new JwtSecurityTokenHandler();

            _logger.LogInformation("Token oluşturma başladı - Kullanıcı: {Username}", user.Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token 1 saat geçerli
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience
            };

            _logger.LogInformation("Token ayarları: Issuer: {Issuer}, Audience: {Audience}, Expires: {Expires}",
                tokenDescriptor.Issuer,
                tokenDescriptor.Audience,
                tokenDescriptor.Expires);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("Token oluşturuldu: {Token}", tokenString);

            return tokenString;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            _logger.LogInformation("Token doğrulama başladı");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token boş veya null");
                return null;
            }

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret ?? "");
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.ValidAudience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                _logger.LogInformation("Token doğrulama başarılı");
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token doğrulama hatası");
                return null;
            }
        }
    }
}
