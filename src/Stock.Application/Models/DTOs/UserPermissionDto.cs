namespace Stock.Application.Models.DTOs
{
    public class UserPermissionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; }
        public string? UserName { get; set; }
        public string? PermissionName { get; set; }
        public string? ResourceType { get; set; }
        public string? ResourceName { get; set; }
        public string? Action { get; set; }
        public string? Description { get; set; }
        public string? Group { get; set; }
    }
} 