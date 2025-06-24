using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EntityId { get; set; } = string.Empty;

        public int? UserId { get; set; }

        [StringLength(200)]
        public string? Path { get; set; }

        public string? Details { get; set; }

        // User ilişkisi
        public virtual User? User { get; set; }
    }
} 