namespace Stock.Application.Models.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string? RoleName { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message { get; set; }
    }
} 