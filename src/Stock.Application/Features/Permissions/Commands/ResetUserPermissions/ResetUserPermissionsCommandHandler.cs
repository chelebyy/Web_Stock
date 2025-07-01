using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.UserPermissions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands.ResetUserPermissions
{
    public class ResetUserPermissionsCommandHandler : IRequestHandler<ResetUserPermissionsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ResetUserPermissionsCommandHandler> _logger;

        public ResetUserPermissionsCommandHandler(IUnitOfWork unitOfWork, ILogger<ResetUserPermissionsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(ResetUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(LogMessages.ResettingPermissionsForUser, request.UserId);

            var userPermissionRepository = _unitOfWork.GetRepository<UserPermission>();
            var spec = new UserPermissionsByUserIdSpecification(request.UserId);
            var userPermissions = await userPermissionRepository.ListAsync(spec, cancellationToken);

            if (userPermissions.Any())
            {
                userPermissionRepository.DeleteRange(userPermissions);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation(LogMessages.PermissionsResetForUser, request.UserId);
            }
            else
            {
                _logger.LogInformation(LogMessages.UserPermissionsResetNoAction, request.UserId);
            }
        }
    }
} 