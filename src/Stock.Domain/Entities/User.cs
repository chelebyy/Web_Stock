using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stock.Domain.Entities.Permissions;

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
        
        [Required]
        [StringLength(50)]
        public string Sicil { get; set; } = string.Empty;

        // Role ilişkisi
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }
        
        // Kullanıcıya özel izinler
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();

        public User()
        {
            IsAdmin = false;
            UserPermissions = new HashSet<UserPermission>();
        }
    }
} 