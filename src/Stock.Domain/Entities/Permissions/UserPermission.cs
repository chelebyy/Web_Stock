using System;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class UserPermission : BaseEntity
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; } = true;

        // Navigation properties
        public User User { get; set; }
        public Permission Permission { get; set; }
    }
} 