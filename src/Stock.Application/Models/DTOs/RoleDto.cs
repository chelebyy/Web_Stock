namespace Stock.Application.Models.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
} 