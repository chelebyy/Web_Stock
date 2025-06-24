using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Yeni rol oluşturmak için kullanılan DTO.
    /// </summary>
    public class CreateRoleDto
    {
        /// <summary>
        /// Yeni rolün adı.
        /// </summary>
        [Required(ErrorMessage = "Rol adı gereklidir.")]
        [StringLength(50, ErrorMessage = "Rol adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Yeni rolün açıklaması (isteğe bağlı).
        /// </summary>
        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string? Description { get; set; }
    }
} 