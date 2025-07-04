using AutoMapper;
using MediatR;
using Stock.Domain.Exceptions;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Roles.Queries.GetRoleById;

/// <summary>
/// Handles the <see cref="GetRoleByIdQuery"/>.
/// </summary>
public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;
    private readonly ICacheService _cacheService;

    public GetRoleByIdQueryHandler(
        IRoleRepository roleRepository,
        IMapper mapper,
        ILogger<GetRoleByIdQueryHandler> logger,
        ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Handles the query to retrieve a role by its ID.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The <see cref="RoleDto"/> if found.</returns>
    /// <exception cref="NotFoundException">Thrown if the role with the specified ID is not found.</exception>
    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"roles:{request.Id}";

        return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
        {
            _logger.LogInformation("{RoleId} ID'li rol veritabanından getiriliyor.", request.Id);
            var spec = new RoleByIdSpecification(request.Id);
            var role = await _roleRepository.FirstOrDefaultAsync(spec, cancellationToken);

            if (role == null)
            {
                _logger.LogWarning("{RoleId} ID'li rol bulunamadı.", request.Id);
                throw new NotFoundException(nameof(Role), request.Id);
            }
            _logger.LogInformation("{RoleId} ID'li rol başarıyla getirildi.", request.Id);

            return _mapper.Map<RoleDto>(role);

        }, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
    }
} 