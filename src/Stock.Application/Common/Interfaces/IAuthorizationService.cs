using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(int userId, string permissionName);
    }
} 