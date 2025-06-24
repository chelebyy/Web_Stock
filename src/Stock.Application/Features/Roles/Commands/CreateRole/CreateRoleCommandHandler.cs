using AutoMapper;
using MediatR;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Roles.Commands.CreateRole;

/// <summary>
/// Handles the <see cref="CreateRoleCommand"/>.
/// </summary>
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to create a new role.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created <see cref="RoleDto"/>.</returns>
    /// <exception cref="ConflictException">Thrown if a role with the same name already exists.</exception>
    /// <exception cref="ValidationException">Thrown if the role fails validation.</exception>
    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("'{Name}' adında yeni rol oluşturuluyor.", request.Name);
        var spec = new RoleByNameSpecification(request.Name);
        var existingRole = await _roleRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (existingRole != null)
        {
            _logger.LogWarning("'{Name}' adında bir rol zaten mevcut.", request.Name);
            throw new ConflictException($"'{request.Name}' adında bir rol zaten mevcut.");
        }

        var roleResult = Role.Create(request.Name, request.Description ?? string.Empty);

        if (!roleResult.IsSuccess)
        {
            _logger.LogError("Rol oluşturulurken validasyon hatası: {Error}", roleResult.Error);
            throw new ValidationException(roleResult.Error);
        }

        var newRole = roleResult.Value;

        await _roleRepository.AddAsync(newRole, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("'{Name}' adlı rol başarıyla oluşturuldu. ID: {RoleId}", newRole.Name, newRole.Id);

        var roleDto = _mapper.Map<RoleDto>(newRole);
        return roleDto;
    }
} 