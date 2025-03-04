using System;

namespace Stock.Application.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string Sicil { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
} 