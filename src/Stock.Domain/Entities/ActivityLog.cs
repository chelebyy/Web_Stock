using System;
using System.ComponentModel.DataAnnotations;

namespace Stock.Domain.Entities
{
    public class ActivityLog : BaseEntity
    {
        public int? UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

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