using MediatR;
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
        private readonly ILoggerManager<UpdateUserPermissionsCommandHandler> _logger;

        public UpdateUserPermissionsCommandHandler(IUnitOfWork unitOfWork, ILoggerManager<UpdateUserPermissionsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarn($"Kullanıcı bulunamadı: ID {request.UserId}");
                throw new NotFoundException($"Kullanıcı bulunamadı: ID {request.UserId}");
            }

            var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();

            _logger.LogInfo($"{request.UserId} ID'li kullanıcı için {request.Permissions.Count} adet izin güncelleniyor.");

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
                        _logger.LogDebug($"Kullanıcı {request.UserId}, İzin {permissionId} güncellendi: IsGranted={isGranted}");
                    }
                }
                else
                {
                    var newUserPermission = UserPermission.Create(request.UserId, permissionId, isGranted);
                    await userPermissionRepo.AddAsync(newUserPermission, cancellationToken);
                    _logger.LogDebug($"Kullanıcı {request.UserId}, İzin {permissionId} eklendi: IsGranted={isGranted}");
                }
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInfo($"Kullanıcı izinleri başarıyla güncellendi: UserId {request.UserId}");

            return Unit.Value;
        }
    }
} 