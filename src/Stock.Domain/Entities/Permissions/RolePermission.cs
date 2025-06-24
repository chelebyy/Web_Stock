using System;
using Stock.Domain.Common;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }

        // Navigation properties
        public Role Role { get; private set; }
        public Permission Permission { get; private set; }

        // EF Core için protected constructor
        protected RolePermission() { }

        // Primary constructor
        private RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        // Factory metodu
        public static RolePermission Create(int roleId, int permissionId)
        {
            return new RolePermission(roleId, permissionId);
        }
    }
} 