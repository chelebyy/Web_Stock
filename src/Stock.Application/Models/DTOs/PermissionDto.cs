using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResourceType { get; set; }
        public string Group { get; set; }
        public bool IsFromRole { get; set; }
        public bool IsCustom { get; set; }
        public string RoleName { get; set; }
    }

    public class PermissionGroupDto
    {
        public string Group { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
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
        public string UserName { get; set; }
        public string PermissionName { get; set; }
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
    }
} 