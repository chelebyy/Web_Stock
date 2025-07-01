using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands.AddMissingPermissions
{
    public class AddMissingPermissionsCommandHandler : IRequestHandler<AddMissingPermissionsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddMissingPermissionsCommandHandler> _logger;

        public AddMissingPermissionsCommandHandler(IUnitOfWork unitOfWork, ILogger<AddMissingPermissionsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(AddMissingPermissionsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogMessages.AddingMissingPermissions);

            await AddPermissionIfNotExistsAsync(PermissionNames.PagesRevirView, "Revir sayfasını görüntüleme", "Sayfa Erişimi", "Page", "Revir", "View", cancellationToken);
            await AddPermissionIfNotExistsAsync(PermissionNames.PagesBilgiIslemView, "Bilgi İşlem sayfasını görüntüleme", "Sayfa Erişimi", "Page", "BilgiIslem", "View", cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully checked and added missing permissions.");
        }

        private async Task AddPermissionIfNotExistsAsync(string name, string description, string group, string resourceType, string resourceName, string action, CancellationToken cancellationToken)
        {
            var permissionRepository = _unitOfWork.GetRepository<Permission>();
            var spec = new PermissionByNameSpecification(name);
            var existingPermission = await permissionRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (existingPermission == null)
            {
                var permissionResult = Permission.Create(name, description, group, resourceType, resourceName, action);
                if (permissionResult.IsSuccess)
                {
                    await permissionRepository.AddAsync(permissionResult.Value, cancellationToken);
                    _logger.LogInformation("Permission '{PermissionName}' did not exist and was added.", name);
                }
                else
                {
                    _logger.LogError("Failed to create permission '{PermissionName}': {Error}", name, permissionResult.Error);
                }
            }
        }
    }
} 