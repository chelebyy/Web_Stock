using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.RolePermissions;
using Stock.Domain.Specifications.UserPermissions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Admin.Commands.CleanupUnusedPermissions
{
    public class CleanupUnusedPermissionsCommandHandler : IRequestHandler<CleanupUnusedPermissionsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CleanupUnusedPermissionsCommandHandler> _logger;

        public CleanupUnusedPermissionsCommandHandler(IUnitOfWork unitOfWork, ILogger<CleanupUnusedPermissionsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(CleanupUnusedPermissionsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Kullanılmayan sayfa izinleri temizleniyor...");

            var permissionsToRemove = new[] {
                "Pages.Reports",
                "Pages.Settings",
                "Pages.StockManagement"
            };

            var allPermissions = await _unitOfWork.Permissions.GetAllAsync(cancellationToken);

            var permissionsToDelete = allPermissions
                .Where(p => permissionsToRemove.Contains(p.Name.ToString()) || p.Name.ToString().StartsWith("Pages.Egitim") || p.Group == "Stok Yönetimi")
                .ToList();

            if (!permissionsToDelete.Any())
            {
                _logger.LogInformation("Temizlenecek kullanılmayan izin bulunamadı.");
                return;
            }

            foreach (var permission in permissionsToDelete)
            {
                await RemovePermission(permission.Id, permission.Name, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Kullanılmayan sayfa izinleri başarıyla temizlendi.");
        }

        private async Task RemovePermission(int permissionId, string permissionName, CancellationToken cancellationToken)
        {
            var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
            var userPermissionsSpec = new UserPermissionsByPermissionIdSpecification(permissionId);
            var userPermissionsToRemove = await userPermissionRepo.ListAsync(userPermissionsSpec, cancellationToken);

            if (userPermissionsToRemove.Any())
            {
                userPermissionRepo.DeleteRange(userPermissionsToRemove);
                _logger.LogInformation($"'{permissionName}' ile ilişkili {userPermissionsToRemove.Count} kullanıcı izni kaldırılmak üzere işaretlendi.");
            }

            var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();
            var rolePermissionsSpec = new RolePermissionsByPermissionIdSpecification(permissionId);
            var rolePermissionsToRemove = await rolePermissionRepo.ListAsync(rolePermissionsSpec, cancellationToken);

            if (rolePermissionsToRemove.Any())
            {
                rolePermissionRepo.DeleteRange(rolePermissionsToRemove);
                _logger.LogInformation($"'{permissionName}' ile ilişkili {rolePermissionsToRemove.Count} rol izni kaldırılmak üzere işaretlendi.");
            }

            var permission = await _unitOfWork.Permissions.GetByIdAsync(permissionId, cancellationToken);
            if (permission != null)
            {
                _unitOfWork.Permissions.Delete(permission);
                _logger.LogInformation($"'{permissionName}' izni kaldırılmak üzere işaretlendi.");
            }
        }
    }
} 