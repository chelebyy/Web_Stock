using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.UserPermissions;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands.RemoveUserPermission
{
    public class RemoveUserPermissionCommandHandler : IRequestHandler<RemoveUserPermissionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveUserPermissionCommandHandler> _logger;

        public RemoveUserPermissionCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserPermissionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(RemoveUserPermissionCommand request, CancellationToken cancellationToken)
        {
            var userPermissionRepository = _unitOfWork.GetRepository<UserPermission>();
            var spec = new UserPermissionSpecification(request.UserId, request.PermissionId);
            var userPermission = await userPermissionRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (userPermission == null)
            {
                // If the link doesn't exist, it's not an error, the desired state is achieved.
                _logger.LogWarning("UserPermission link not found for User {UserId} and Permission {PermissionId}. No action taken.", request.UserId, request.PermissionId);
                return;
            }

            userPermissionRepository.Delete(userPermission);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(LogMessages.PermissionRemovedFromUser, request.PermissionId, request.UserId);
        }
    }
} 