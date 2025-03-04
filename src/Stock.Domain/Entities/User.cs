using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Entities
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
        
        [StringLength(50)]
        public string Sicil { get; set; } = string.Empty;

        // Role ilişkisi
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public User()
        {
            IsAdmin = false;
        }
    }
} 