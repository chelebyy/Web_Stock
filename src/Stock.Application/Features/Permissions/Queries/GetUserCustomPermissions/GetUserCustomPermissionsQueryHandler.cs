using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Application.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.UserPermissions;

namespace Stock.Application.Features.Permissions.Queries.GetUserCustomPermissions
{
    public class GetUserCustomPermissionsQueryHandler : IRequestHandler<GetUserCustomPermissionsQuery, List<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager<GetUserCustomPermissionsQueryHandler> _logger;

        public GetUserCustomPermissionsQueryHandler(IUnitOfWork unitOfWork, ILoggerManager<GetUserCustomPermissionsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<List<PermissionDto>> Handle(GetUserCustomPermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
                var spec = new UserCustomPermissionsSpecification(request.UserId);
                var userPermissions = await userPermissionRepo.ListAsync(spec, cancellationToken);

                return userPermissions.Select(up => new PermissionDto
                {
                    Id = up.PermissionId,
                    IsGranted = up.IsGranted,
                    Name = up.Permission?.Name ?? string.Empty,
                    ResourceType = up.Permission?.ResourceType ?? string.Empty,
                    Description = up.Permission?.Description ?? string.Empty,
                    Group = up.Permission?.Group ?? string.Empty,
                    IsCustom = true
                }).OrderBy(p => p.Group).ThenBy(p => p.Name).ToList();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingCustomUserPermissions, request.UserId);
                // Depending on the desired behavior, you might want to rethrow,
                // or return an empty list/null, or a result object indicating failure.
                // Here, returning an empty list to avoid crashing the client.
                return new List<PermissionDto>();
            }
        }
    }
} 