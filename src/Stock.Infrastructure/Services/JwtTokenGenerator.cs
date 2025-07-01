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
using MediatR;
using Stock.Application.Constants;
using Stock.Application.Features.Permissions.Queries.GetPermissionsByUserId;
using Stock.Application.Models.DTOs;

namespace Stock.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;

        public JwtTokenGenerator(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
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
                new Claim(ClaimTypes.Name, $"{user.FullName.Adi} {user.FullName.Soyadi}"),
                new Claim(ClaimTypes.Role, user.IsAdmin ? RoleNames.Admin : (user.Role?.Name ?? RoleNames.User)),
                new Claim(JwtClaimTypes.Sicil, user.Sicil ?? string.Empty)
            };

            if (user.Role != null)
            {
                claims.Add(new Claim(JwtClaimTypes.RoleName, user.Role.Name));
                claims.Add(new Claim(JwtClaimTypes.RoleId, user.Role.Id.ToString()));
            }
            
            var permissionsQuery = new GetPermissionsByUserIdQuery(user.Id);
            var permissions = await _mediator.Send(permissionsQuery);

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(JwtClaimTypes.Permission, permission.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
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