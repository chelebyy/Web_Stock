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

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
    }
} 