using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePermissionCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public UpdatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdatePermissionCommandHandler> logger,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ID: {PermissionId} olan izin güncelleniyor.", request.Id);

            var permissionRepository = _unitOfWork.GetRepository<Permission>();
            var spec = new PermissionByIdSpecification(request.Id);
            var permission = await permissionRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (permission == null)
            {
                _logger.LogWarning("Güncellenecek izin bulunamadı: ID {PermissionId}", request.Id);
                return Result.Failure($"Permission with ID {request.Id} not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var nameUpdateResult = permission.UpdateName(request.Name);
                if (!nameUpdateResult.IsSuccess)
                {
                    _logger.LogWarning("İzin adı güncelleme başarısız (Domain Validation): {Error}", nameUpdateResult.Error);
                    return Result.Failure(nameUpdateResult.Error);
                }
            }

            var descriptionUpdateResult = permission.UpdateDescription(request.Description);
            if (!descriptionUpdateResult.IsSuccess)
            {
                _logger.LogWarning("İzin açıklaması güncelleme başarısız (Domain Validation): {Error}", descriptionUpdateResult.Error);
                return Result.Failure(descriptionUpdateResult.Error);
            }

            var groupUpdateResult = permission.UpdateGroup(request.Group);
            if (!groupUpdateResult.IsSuccess)
            {
                _logger.LogWarning("İzin grubu güncelleme başarısız (Domain Validation): {Error}", groupUpdateResult.Error);
                return Result.Failure(groupUpdateResult.Error);
            }

            var resourceTypeUpdateResult = permission.UpdateResourceType(request.ResourceType);
            if (!resourceTypeUpdateResult.IsSuccess)
            {
                _logger.LogWarning("İzin kaynak tipi güncelleme başarısız (Domain Validation): {Error}", resourceTypeUpdateResult.Error);
                return Result.Failure(resourceTypeUpdateResult.Error);
            }

            permission.UpdateResourceName(request.ResourceName);
            permission.UpdateAction(request.Action);

            // The entity is already being tracked by EF Core.
            // No need to call an update method on the repository.
            
            var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (saveResult <= 0)
            {
                _logger.LogError("İzin güncellenirken veritabanına kaydetme başarısız: ID {PermissionId}", request.Id);
                return Result.Failure("Failed to update the permission in the database.");
            }

            // İzin önbelleğini temizle
            await _cacheService.RemoveAsync(Application.Common.Constants.CacheKeys.PermissionsAll);
            _logger.LogInformation("İzin listesi önbelleği temizlendi.");

            _logger.LogInformation("ID: {PermissionId} olan izin başarıyla güncellendi.", request.Id);
            return Result.Success();
        }
    }
} 