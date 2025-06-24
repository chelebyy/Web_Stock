using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Models.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifre gereklidir")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre gereklidir")]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre tekrarı gereklidir")]
        [Compare(nameof(NewPassword), ErrorMessage = "Yeni şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 