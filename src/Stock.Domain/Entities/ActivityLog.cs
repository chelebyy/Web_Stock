using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Entities
{
    public class ActivityLog : BaseEntity
    {
        public int? UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; }
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string? AdditionalInfo { get; set; }

        public bool IsSynchronized { get; set; }

        // Navigation property
        public virtual User? User { get; set; }

        public ActivityLog()
        {
            Timestamp = DateTime.UtcNow;
            IsSynchronized = false;
        }
    }
} 