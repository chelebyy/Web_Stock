using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAPI.Models
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string EntityId { get; set; }

        [Required]
        public string EntityType { get; set; }

        [Required]
        public string Path { get; set; }

        public string? Details { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
