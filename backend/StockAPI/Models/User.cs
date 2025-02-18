using System.ComponentModel.DataAnnotations;

namespace StockAPI.Models
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // Role ilişkisi
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
