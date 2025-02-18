using System.ComponentModel.DataAnnotations;

namespace StockAPI.Models
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
} 