using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs; // DTO namespace'i düzeltildi
using Stock.Domain.Interfaces; // IUnitOfWork için eklendi
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Entities.Permissions; // Permission için eklendi
using Stock.Domain.Specifications; // BaseSpecification için eklendi

namespace Stock.Application.Features.Permissions.Queries.GetAllPermissions;

/// <summary>
/// Handles the <see cref="GetAllPermissionsQuery"/>.
/// </summary>
public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetAllPermissionsQueryHandler> _logger;
    private const string CacheKey = "permissions_all";

    public GetAllPermissionsQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ICacheService cacheService, 
        ILogger<GetAllPermissionsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the query to retrieve all permissions.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of <see cref="PermissionDto"/>.</returns>
    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetOrCreateAsync(CacheKey, async () =>
        {
            _logger.LogInformation("Cache miss for key: {CacheKey}. Fetching permissions from database.", CacheKey);
            var spec = new AllPermissionsSpecification();
            var permissions = await _unitOfWork.GetRepository<Permission>().ListAsync(spec, cancellationToken);
            var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
            _logger.LogInformation("{Count} permissions fetched from database and cached.", permissionDtos.Count());
            return permissionDtos;
        },
        slidingExpiration: TimeSpan.FromHours(1)); // Permissions change less frequently
    }
} 