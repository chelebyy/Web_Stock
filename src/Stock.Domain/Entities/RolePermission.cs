using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Domain.Entities
{
    public class RolePermission
    {
        [Key]
        [Column(Order = 0)]
        public int RoleId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int PermissionId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; } = null!;
    }
} 