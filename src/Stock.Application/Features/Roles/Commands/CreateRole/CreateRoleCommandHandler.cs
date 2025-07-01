using MediatR;
using AutoMapper;


using Stock.Domain.Entities;
using Stock.Domain.Common;
using Stock.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Stock.Application.Features.Roles.DTOs;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoleCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRoleCommandHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var roleNameResult = RoleName.Create(request.Name);
            if (!roleNameResult.IsSuccess)
            {
                throw new System.Exception(roleNameResult.Error);
            }

            var roleResult = Role.Create(roleNameResult.Value, request.Description);
            if (!roleResult.IsSuccess)
            {
                throw new System.Exception(roleResult.Error);
            }

            var role = roleResult.Value;

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Yeni rol başarıyla oluşturuldu: {RoleName}", role.Name);

            // Invalidate the cache for all role list pages
            var prefix = "roles_page";
            await _cacheService.RemoveByPrefixAsync(prefix);
            _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", prefix);

            return _mapper.Map<RoleDto>(role);
        }
    }
}