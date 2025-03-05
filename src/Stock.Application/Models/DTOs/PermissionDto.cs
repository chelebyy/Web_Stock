using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string? ResourceType { get; set; }
        public string? ResourceName { get; set; }
        public string? Action { get; set; }
        public bool IsFromRole { get; set; }
        public bool IsCustom { get; set; }
        public string? RoleName { get; set; }
    }

    public class PermissionGroupDto
    {
        public string Group { get; set; } = string.Empty;
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
} 