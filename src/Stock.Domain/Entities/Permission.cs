using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Entities
{
    public class Permission : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string Group { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ResourceType { get; set; } // "Page", "Function" gibi

        [StringLength(50)]
        public string? ResourceName { get; set; } // "Revir", "BilgiIslem" gibi

        [StringLength(50)]
        public string? Action { get; set; } // "View", "Edit", "Create", "Delete" gibi

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
        
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();
    }
} 