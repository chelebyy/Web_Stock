using System.Text.Json;

namespace StockAPI.Models
{
    public class AuditLog : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? OldData { get; set; }
        public string? NewData { get; set; }
        public string? Details { get; set; }

        public virtual User? User { get; set; }

        public T? GetOldData<T>() => OldData != null ? JsonSerializer.Deserialize<T>(OldData) : default;
        public T? GetNewData<T>() => NewData != null ? JsonSerializer.Deserialize<T>(NewData) : default;
    }
}
