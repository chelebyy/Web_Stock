using System;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
} 