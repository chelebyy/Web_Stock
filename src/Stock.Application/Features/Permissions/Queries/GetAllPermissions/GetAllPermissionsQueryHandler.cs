using AutoMapper;
using MediatR;
using Stock.Application.Models.DTOs; // DTO namespace'i düzeltildi
using Stock.Domain.Interfaces; // IUnitOfWork için eklendi
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

    public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve all permissions.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of <see cref="PermissionDto"/>.</returns>
    public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        // Tüm izinleri getirmek için oluşturulan spesifikasyonu kullan
        var spec = new AllPermissionsSpecification(); // BaseSpecification yerine
        var permissions = await _unitOfWork.GetRepository<Permission>().ListAsync(spec, cancellationToken);

        var permissionDtos = _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        return permissionDtos;
    }
} 