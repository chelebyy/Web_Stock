using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.RolePermissions;
using Stock.Domain.Specifications.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Roles.Commands.UpdateRolePermissions
{
    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRolePermissionsCommandHandler> _logger;
        private readonly ICacheService _cacheService;
        private const string CacheKey = "roles_all";

        public UpdateRolePermissionsCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRolePermissionsCommandHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Unit> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                _logger.LogWarning($"Rol bulunamadı: ID {request.RoleId}");
                throw new NotFoundException($"Rol bulunamadı: ID {request.RoleId}");
            }

            var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();

            var existingPermissionsSpec = new RolePermissionsByRoleIdSpecification(request.RoleId);
            var existingPermissions = await rolePermissionRepo.ListAsync(existingPermissionsSpec, cancellationToken);

            if (existingPermissions.Any())
            {
                rolePermissionRepo.DeleteRange(existingPermissions);
                _logger.LogInformation($"{existingPermissions.Count} adet mevcut izin ilişkisi silinmek üzere işaretlendi: RoleId {request.RoleId}");
            }

            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                var newPermissions = request.PermissionIds
                    .Select(permissionId => RolePermission.Create(request.RoleId, permissionId))
                    .ToList();
                
                await rolePermissionRepo.AddRangeAsync(newPermissions, cancellationToken);
                _logger.LogInformation($"{newPermissions.Count} adet yeni izin ilişkisi ekleniyor: RoleId {request.RoleId}");
            }

            _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Rol izinleri başarıyla güncellendi: RoleId {request.RoleId}");

            // Invalidate the cache for all roles list
            await _cacheService.RemoveAsync(CacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {CacheKey}", CacheKey);
            
            // Invalidate the cache for the specific role's permissions
            var permissionsCacheKey = $"role-permissions:{request.RoleId}";
            await _cacheService.RemoveAsync(permissionsCacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {PermissionsCacheKey}", permissionsCacheKey);
            
            // Invalidate the cache for the specific role itself
            var roleCacheKey = $"roles:{request.RoleId}";
            await _cacheService.RemoveAsync(roleCacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {RoleCacheKey}", roleCacheKey);

            var usersInRoleSpec = new UsersByRoleIdSpecification(request.RoleId);
            var usersInRole = await _unitOfWork.Users.ListAsync(usersInRoleSpec, cancellationToken);

            if (usersInRole.Any())
            {
                _logger.LogInformation("Invalidating caches for {UserCount} users in role {RoleId}.", usersInRole.Count, request.RoleId);
                foreach (var user in usersInRole)
                {
                    var userPermissionsCacheKey = $"user-permissions:{user.Id}";
                    await _cacheService.RemoveAsync(userPermissionsCacheKey).ConfigureAwait(false);
                    _logger.LogInformation("Cache invalidated for user {UserId} with key: {UserPermissionsCacheKey}", user.Id, userPermissionsCacheKey);
                }
            }

            return Unit.Value;
        }
    }
} 