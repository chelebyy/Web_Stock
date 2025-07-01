using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.UserPermissions;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Commands.UpdateUserPermissions
{
    public class UpdateUserPermissionsCommandHandler : IRequestHandler<UpdateUserPermissionsCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserPermissionsCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public UpdateUserPermissionsCommandHandler(
            IUnitOfWork unitOfWork, 
            ILogger<UpdateUserPermissionsCommandHandler> logger, 
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Unit> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: ID {UserId}", request.UserId);
                throw new NotFoundException($"Kullanıcı bulunamadı: ID {request.UserId}");
            }

            var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();

            _logger.LogInformation("{UserId} ID'li kullanıcı için {PermissionCount} adet izin güncelleniyor.", request.UserId, request.Permissions.Count);

            foreach (var permission in request.Permissions)
            {
                var permissionId = permission.Key;
                var isGranted = permission.Value;

                var spec = new UserPermissionSpecification(request.UserId, permissionId);
                var existingPermission = await userPermissionRepo.FirstOrDefaultAsync(spec, cancellationToken);

                if (existingPermission != null)
                {
                    if (existingPermission.IsGranted != isGranted)
                    {
                        existingPermission.UpdateGrantStatus(isGranted);
                        userPermissionRepo.Update(existingPermission);
                        _logger.LogDebug("Kullanıcı {UserId}, İzin {PermissionId} güncellendi: IsGranted={IsGranted}", request.UserId, permissionId, isGranted);
                    }
                }
                else
                {
                    var newUserPermission = UserPermission.Create(request.UserId, permissionId, isGranted);
                    await userPermissionRepo.AddAsync(newUserPermission, cancellationToken);
                    _logger.LogDebug("Kullanıcı {UserId}, İzin {PermissionId} eklendi: IsGranted={IsGranted}", request.UserId, permissionId, isGranted);
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Kullanıcı izinleri başarıyla güncellendi: UserId {UserId}", request.UserId);

            var cacheKey = $"user-permissions:{request.UserId}";
            await _cacheService.RemoveAsync(cacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for key: {CacheKey}", cacheKey);

            return Unit.Value;
        }
    }
} 