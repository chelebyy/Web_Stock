using System.Collections.Generic;

namespace Stock.Application.Models.DTOs
{
    public class PermissionGroupDto
    {
        public string Group { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
} 