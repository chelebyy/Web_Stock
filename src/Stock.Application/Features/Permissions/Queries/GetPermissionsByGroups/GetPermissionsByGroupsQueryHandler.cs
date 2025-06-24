using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByGroups
{
    public class GetPermissionsByGroupsQueryHandler : IRequestHandler<GetPermissionsByGroupsQuery, IEnumerable<PermissionGroupDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPermissionsByGroupsQueryHandler> _logger;

        public GetPermissionsByGroupsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPermissionsByGroupsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionGroupDto>> Handle(GetPermissionsByGroupsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting permissions grouped by their group.");

            var permissionRepository = _unitOfWork.GetRepository<Permission>();
            var spec = new AllPermissionsSpecification();
            var permissions = await permissionRepository.ListAsync(spec, cancellationToken);

            var groupedPermissions = permissions
                .GroupBy(p => p.Group ?? PermissionGroupNames.Other)
                .Select(g => new PermissionGroupDto
                {
                    Group = g.Key,
                    Permissions = _mapper.Map<List<PermissionDto>>(g.OrderBy(p => p.Name).ToList())
                })
                .OrderBy(g => g.Group);

            return groupedPermissions;
        }
    }
} 