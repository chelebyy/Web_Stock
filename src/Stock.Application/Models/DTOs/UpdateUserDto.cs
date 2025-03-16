using System.ComponentModel.DataAnnotations;

namespace Stock.Application.Models.DTOs
{
    /// <summary>
    /// Kullanıcı güncelleme işlemi için DTO
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// Kullanıcının admin olup olmadığı
        /// </summary>
        public bool IsAdmin { get; set; }
        
        /// <summary>
        /// Kullanıcının rol ID'si
        /// </summary>
        public int? RoleId { get; set; }
        
        /// <summary>
        /// Kullanıcının aktif olup olmadığı
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
} 