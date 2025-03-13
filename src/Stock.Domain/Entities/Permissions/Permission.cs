using System;
using System.Collections.Generic;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string Action { get; set; }
        public string Group { get; set; }

        // Navigation properties
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
} 