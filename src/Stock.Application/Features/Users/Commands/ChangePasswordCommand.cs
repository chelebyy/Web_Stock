using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Kullanıcı şifresini değiştirmek için kullanılan komut sınıfı.
    /// </summary>
    public class ChangePasswordCommand : IRequest<bool>
    {
        /// <summary>
        /// Şifresi değiştirilecek kullanıcının ID'si.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Kullanıcının mevcut şifresi.
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// Kullanıcının yeni şifresi.
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Yeni şifrenin onayı.
        /// </summary>
        [Required]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 