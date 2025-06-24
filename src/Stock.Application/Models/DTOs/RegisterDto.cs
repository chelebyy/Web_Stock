namespace Stock.Application.Models.DTOs
{
    public class RegisterDto
    {
        public string Adi { get; set; } = string.Empty;
        public string Soyadi { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Sicil { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }
} 