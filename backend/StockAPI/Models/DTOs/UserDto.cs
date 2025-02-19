using System;

namespace StockAPI.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }

    public class UpdateUserDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }

    public class LoginDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
