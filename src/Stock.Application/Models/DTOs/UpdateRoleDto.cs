using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Rol güncellemek için kullanılan DTO.
    /// </summary>
    public class UpdateRoleDto
    {
        // ID route'dan alınacağı için burada genellikle olmaz, ancak Command nesnesinde olabilir.

        /// <summary>
        /// Güncellenmiş rol adı.
        /// </summary>
        [Required(ErrorMessage = "Rol adı gereklidir.")]
        [StringLength(50, ErrorMessage = "Rol adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Güncellenmiş rol açıklaması (isteğe bağlı).
        /// </summary>
        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        public string? Description { get; set; }
    }
} 