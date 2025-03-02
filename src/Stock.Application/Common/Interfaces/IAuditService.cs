using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string entityType, string entityId, string userId, string path, string details);
    }
} 