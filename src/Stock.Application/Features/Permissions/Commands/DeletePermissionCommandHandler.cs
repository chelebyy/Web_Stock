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
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePermissionCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public DeletePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<DeletePermissionCommandHandler> logger,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ID: {PermissionId} olan izin siliniyor.", request.Id);

            var permissionRepository = _unitOfWork.GetRepository<Permission>();
            var spec = new PermissionByIdSpecification(request.Id);
            var permission = await permissionRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (permission == null)
            {
                _logger.LogWarning("Silinecek izin bulunamadı: ID {PermissionId}", request.Id);
                return Result.Failure($"Permission with ID {request.Id} not found.");
            }

            try
            {
                await permissionRepository.DeleteAsync(permission, cancellationToken);
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("İzin silinirken veritabanına kaydetme başarısız: ID {PermissionId}", request.Id);
                    return Result.Failure("Failed to delete the permission from the database.");
                }

                // İzin önbelleğini temizle
                await _cacheService.RemoveAsync(Application.Common.Constants.CacheKeys.PermissionsAll);
                _logger.LogInformation("İzin listesi önbelleği temizlendi.");

                _logger.LogInformation("ID: {PermissionId} olan izin başarıyla silindi.", request.Id);
                return Result.Success();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "İzin silinirken bir hata oluştu: ID {PermissionId}", request.Id);
                return Result.Failure($"An error occurred while deleting the permission: {ex.Message}");
            }
        }
    }
} 