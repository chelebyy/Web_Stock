using System;

namespace Stock.Domain.Entities
{
    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; } // İzin verildi mi yoksa açıkça reddedildi mi
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
} 