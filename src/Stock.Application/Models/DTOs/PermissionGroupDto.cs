using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class PermissionGroupDto
    {
        public int Id { get; set; }
        public string Group { get; set; } = null!;
        public List<PermissionDto> Permissions { get; set; } = new();
    }
} 