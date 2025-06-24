using System;
using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Sicil { get; set; } = string.Empty;
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string? RoleName { get; set; }
        public int? RoleId { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }
} 