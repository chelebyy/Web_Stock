using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;

        public JwtTokenGenerator(IConfiguration configuration, IPermissionService permissionService)
        {
            _configuration = configuration;
            _permissionService = permissionService;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                new Claim("sicil", user.Sicil)
            };

            if (user.Role != null)
            {
                claims.Add(new Claim("RoleName", user.Role.Name));
                claims.Add(new Claim("RoleId", user.Role.Id.ToString()));
            }
            
            // Kullanıcının tüm izinlerini ekle (rol izinleri + özel izinler)
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Geriye dönük uyumluluk için senkron metodu da tutalım
        public string GenerateToken(User user)
        {
            return GenerateTokenAsync(user).GetAwaiter().GetResult();
        }
    }
} 