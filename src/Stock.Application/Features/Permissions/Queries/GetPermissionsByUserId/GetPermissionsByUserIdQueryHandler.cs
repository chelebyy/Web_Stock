using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Constants;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.UserPermissions;
using Stock.Domain.Specifications.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionsByUserId
{
    public class GetPermissionsByUserIdQueryHandler : IRequestHandler<GetPermissionsByUserIdQuery, IEnumerable<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPermissionsByUserIdQueryHandler> _logger;

        public GetPermissionsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPermissionsByUserIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all permissions for user with ID: {UserId}", request.UserId);

            var userRepo = _unitOfWork.GetRepository<User>();
            var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();

            var userSpec = new UserWithRoleAndPermissionsSpecification(request.UserId);
            var user = await userRepo.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found.", request.UserId);
                return Enumerable.Empty<PermissionDto>();
            }

            var allPermissions = new Dictionary<int, PermissionDto>();

            // Rol izinlerini ekle
            if (user.Role?.RolePermissions != null)
            {
                foreach (var rp in user.Role.RolePermissions)
                {
                    if (rp.Permission != null && !allPermissions.ContainsKey(rp.Permission.Id))
                    {
                        allPermissions.Add(rp.Permission.Id, MapToDto(rp.Permission, true, user.Role.Name));
                    }
                }
            }

            // Kullanıcının özel izinlerini işle
            var userPermissionsSpec = new UserPermissionsWithDetailsSpecification(request.UserId);
            var userPermissions = await userPermissionRepo.ListAsync(userPermissionsSpec, cancellationToken);

            foreach (var up in userPermissions)
            {
                if (up.Permission != null)
                {
                    if (allPermissions.TryGetValue(up.Permission.Id, out var existingDto))
                    {
                        // Eğer rolünden gelen izin, kullanıcı tarafından özel olarak kaldırıldıysa
                        if (!up.IsGranted)
                        {
                            existingDto.IsGranted = false;
                            existingDto.IsCustom = true;
                        }
                        // Eğer rolünden gelen izin, kullanıcı tarafından özel olarak da eklendiyse (gereksiz ama olabilir)
                        else if (existingDto.IsFromRole)
                        {
                            existingDto.IsCustom = true;
                        }
                    }
                    // Eğer rolünden gelmeyen ve kullanıcıya özel olarak eklenmiş bir izinse
                    else if (up.IsGranted)
                    {
                        var newDto = MapToDto(up.Permission, false);
                        newDto.IsCustom = true;
                        allPermissions.Add(up.Permission.Id, newDto);
                    }
                }
            }

            return allPermissions.Values.OrderBy(p => p.Group).ThenBy(p => p.Name);
        }

        private PermissionDto MapToDto(Permission permission, bool isFromRole = false, string? roleName = null)
        {
            var dto = _mapper.Map<PermissionDto>(permission);
            dto.IsFromRole = isFromRole;
            dto.RoleName = roleName;
            dto.IsGranted = true; // Başlangıçta tüm izinler verilmiş kabul edilir
            dto.IsCustom = !isFromRole; // Rolünden gelmiyorsa custom'dır
            return dto;
        }
    }
} 