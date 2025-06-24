using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePermissionCommandHandler> _logger;

        public UpdatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdatePermissionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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

            // Name is not updated as it's a key identifier.
            // If name changes are needed, a different pattern (like retiring old and creating new) should be used.

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

            _logger.LogInformation("ID: {PermissionId} olan izin başarıyla güncellendi.", request.Id);
            return Result.Success();
        }
    }
} 