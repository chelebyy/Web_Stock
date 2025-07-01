using System;

namespace Stock.Application.Models.DTOs
{
    public class ActivityLogDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string? AdditionalInfo { get; set; }
    }
} 