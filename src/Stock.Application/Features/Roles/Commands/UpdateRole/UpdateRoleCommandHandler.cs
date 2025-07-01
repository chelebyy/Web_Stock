using MediatR;

using Stock.Domain.Entities;
using Stock.Domain.Common;
using Stock.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Roles.Commands.UpdateRole;

/// <summary>
/// Handles the <see cref="UpdateRoleCommand"/>.
/// </summary>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRoleCommandHandler> _logger;
    private readonly ICacheService _cacheService;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRoleCommandHandler> logger, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cacheService = cacheService;
    }

    /// <summary>
    /// Handles the command to update an existing role.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if update was successful.</returns>
    /// <exception cref="Exception">Thrown if the role to update is not found, validation fails, or an error occurs.</exception>
    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.Id);

        if (role == null)
        {
            throw new Exception($"Role with ID {request.Id} not found.");
        }

        var roleNameResult = RoleName.Create(request.Name);
        if (!roleNameResult.IsSuccess)
        {
            throw new Exception(roleNameResult.Error);
        }

        var updateNameResult = role.UpdateName(roleNameResult.Value);
        if (!updateNameResult.IsSuccess)
        {
            throw new Exception(updateNameResult.Error);
        }

        var updateDescriptionResult = role.UpdateDescription(request.Description);
        if (!updateDescriptionResult.IsSuccess)
        {
            throw new Exception(updateDescriptionResult.Error);
        }

        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Rol başarıyla güncellendi: {RoleId}", request.Id);

        // Invalidate both the list cache and the specific item cache
        var listCachePrefix = "roles_page";
        await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
        _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

        var specificCacheKey = $"roles:{request.Id}";
        await _cacheService.RemoveAsync(specificCacheKey).ConfigureAwait(false);
        _logger.LogInformation("Cache invalidated for key: {SpecificCacheKey}", specificCacheKey);

        return Unit.Value;
    }
} 