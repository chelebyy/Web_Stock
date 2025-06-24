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
using Stock.Application.Constants;

namespace Stock.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
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
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is not configured in appsettings.");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Adi} {user.Soyadi}"),
                new Claim(ClaimTypes.Role, user.IsAdmin ? RoleNames.Admin : (user.Role?.Name ?? RoleNames.User)),
                new Claim(JwtClaimTypes.Sicil, user.Sicil ?? string.Empty)
            };

            if (user.Role != null)
            {
                claims.Add(new Claim(JwtClaimTypes.RoleName, user.Role.Name));
                claims.Add(new Claim(JwtClaimTypes.RoleId, user.Role.Id.ToString()));
            }
            
            var permissions = await _permissionService.GetPermissionsByUserIdAsync(user.Id);
            foreach (var permission in permissions.Where(p => p.IsGranted))
            {
                claims.Add(new Claim(JwtClaimTypes.Permission, permission.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpirationHours"] ?? "1")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            return GenerateTokenAsync(user).GetAwaiter().GetResult();
        }
    }
} 