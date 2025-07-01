using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.RolePermissions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByRoleId
{
    public class GetPermissionsByRoleIdQueryHandler : IRequestHandler<GetPermissionsByRoleIdQuery, IEnumerable<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPermissionsByRoleIdQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetPermissionsByRoleIdQueryHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<GetPermissionsByRoleIdQueryHandler> logger,
            ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"role-permissions:{request.RoleId}";
            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Getting permissions from database for role with ID: {RoleId}", request.RoleId);

                var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();
                var spec = new PermissionsByRoleIdSpecification(request.RoleId);
                var rolePermissions = await rolePermissionRepo.ListAsync(spec, cancellationToken);

                return rolePermissions
                    .Where(rp => rp.Permission != null)
                    .Select(rp => _mapper.Map<PermissionDto>(rp.Permission))
                    .ToList();
            }, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }
    }
} 