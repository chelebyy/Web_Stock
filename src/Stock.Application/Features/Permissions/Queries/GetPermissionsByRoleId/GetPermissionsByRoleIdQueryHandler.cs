using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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

        public GetPermissionsByRoleIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPermissionsByRoleIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByRoleIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting permissions for role with ID: {RoleId}", request.RoleId);

            var rolePermissionRepo = _unitOfWork.GetRepository<RolePermission>();
            var spec = new PermissionsByRoleIdSpecification(request.RoleId);
            var rolePermissions = await rolePermissionRepo.ListAsync(spec, cancellationToken);

            return rolePermissions
                .Where(rp => rp.Permission != null)
                .Select(rp => _mapper.Map<PermissionDto>(rp.Permission))
                .ToList();
        }
    }
} 