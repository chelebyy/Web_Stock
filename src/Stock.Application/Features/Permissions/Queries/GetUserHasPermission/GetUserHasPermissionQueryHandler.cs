using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Specifications;
using Stock.Domain.Specifications.UserPermissions;
using Stock.Domain.Specifications.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Permissions.Queries.GetUserHasPermission
{
    public class GetUserHasPermissionQueryHandler : IRequestHandler<GetUserHasPermissionQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserHasPermissionQueryHandler> _logger;

        public GetUserHasPermissionQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserHasPermissionQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(GetUserHasPermissionQuery request, CancellationToken cancellationToken)
        {
            // 1. Check for a direct user-specific permission first.
            var userPermissionSpec = new UserPermissionByNameSpecification(request.UserId, request.PermissionName);
            var directPermission = await _unitOfWork.GetRepository<UserPermission>().FirstOrDefaultAsync(userPermissionSpec);

            if (directPermission != null)
            {
                _logger.LogInformation("Direct permission found for user {UserId} and permission {PermissionName}. IsGranted: {IsGranted}", request.UserId, request.PermissionName, directPermission.IsGranted);
                // If a direct permission exists, it overrides any role permission.
                return directPermission.IsGranted;
            }

            // 2. If no direct permission, check the user's role.
            var userWithRoleSpec = new UserWithRoleAndPermissionsSpecification(request.UserId);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userWithRoleSpec);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for permission check.", request.UserId);
                return false;
            }

            if (user.Role == null)
            {
                _logger.LogInformation("User {UserId} has no role, and no direct permission for {PermissionName}.", request.UserId, request.PermissionName);
                return false;
            }

            // Check if the permission is granted through the role.
            var hasPermissionFromRole = user.Role.RolePermissions.Any(rp => rp.Permission?.Name.Value == request.PermissionName);

            if (hasPermissionFromRole)
            {
                _logger.LogInformation("User {UserId} has permission {PermissionName} from role {RoleName}.", request.UserId, request.PermissionName, user.Role.Name.Value);
            }
            else
            {
                _logger.LogInformation("User {UserId} does not have permission {PermissionName} from role or directly.", request.UserId, request.PermissionName);
            }

            return hasPermissionFromRole;
        }
    }
} 