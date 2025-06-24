using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.RolePermissions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Roles.Commands.UpdateRolePermissions
{
    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager<UpdateRolePermissionsCommandHandler> _logger;

        public UpdateRolePermissionsCommandHandler(IUnitOfWork unitOfWork, ILoggerManager<UpdateRolePermissionsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                _logger.LogWarn($"Rol bulunamadı: ID {request.RoleId}");
                throw new NotFoundException($"Rol bulunamadı: ID {request.RoleId}");
            }

            var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();

            var existingPermissionsSpec = new RolePermissionsByRoleIdSpecification(request.RoleId);
            var existingPermissions = await rolePermissionRepo.ListAsync(existingPermissionsSpec, cancellationToken);

            if (existingPermissions.Any())
            {
                rolePermissionRepo.DeleteRange(existingPermissions);
                _logger.LogInfo($"{existingPermissions.Count} adet mevcut izin ilişkisi silinmek üzere işaretlendi: RoleId {request.RoleId}");
            }

            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                var newPermissions = request.PermissionIds
                    .Select(permissionId => RolePermission.Create(request.RoleId, permissionId))
                    .ToList();
                
                await rolePermissionRepo.AddRangeAsync(newPermissions, cancellationToken);
                _logger.LogInfo($"{newPermissions.Count} adet yeni izin ilişkisi ekleniyor: RoleId {request.RoleId}");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInfo($"Rol izinleri başarıyla güncellendi: RoleId {request.RoleId}");

            return Unit.Value;
        }
    }
} 