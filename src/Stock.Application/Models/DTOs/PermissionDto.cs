using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public bool IsGranted { get; set; } = true;
        public bool IsFromRole { get; set; }
        public bool IsCustom { get; set; }
        public string? RoleName { get; set; }
    }

    public class AssignPermissionsRequest
    {
        public List<int> PermissionIds { get; set; } = new List<int>();
    }

    public class UserPermissionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; }
        public string AdiSoyadi { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
    }
} 