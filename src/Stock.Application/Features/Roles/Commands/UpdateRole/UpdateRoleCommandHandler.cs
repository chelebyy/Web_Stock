using AutoMapper;
using MediatR;
using Stock.Domain.Exceptions;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Roles.Commands.UpdateRole;

/// <summary>
/// Handles the <see cref="UpdateRoleCommand"/>.
/// </summary>
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateRoleCommandHandler> _logger;

    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to update an existing role.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if update was successful.</returns>
    /// <exception cref="NotFoundException">Thrown if the role to update is not found.</exception>
    /// <exception cref="ConflictException">Thrown if another role with the same name already exists.</exception>
    /// <exception cref="ValidationException">Thrown if the role fails validation.</exception>
    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{RoleId} ID'li rol güncelleniyor.", request.Id);
        var spec = new RoleByIdSpecification(request.Id);
        var roleToUpdate = await _roleRepository.FirstOrDefaultAsync(spec, cancellationToken);

        if (roleToUpdate == null)
        {
            _logger.LogWarning("{RoleId} ID'li rol bulunamadı.", request.Id);
            throw new NotFoundException(nameof(Role), request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Name) && roleToUpdate.Name != request.Name)
        {
            var nameCheckSpec = new RoleByNameSpecification(request.Name);
            var existingRoleWithSameName = await _roleRepository.FirstOrDefaultAsync(nameCheckSpec, cancellationToken);
            if (existingRoleWithSameName != null && existingRoleWithSameName.Id != request.Id)
            {
                _logger.LogWarning("'{Name}' adında başka bir rol zaten mevcut (ID: {ExistingRoleId}). Güncellenmek istenen rol ID: {RequestedRoleId}", request.Name, existingRoleWithSameName.Id, request.Id);
                throw new ConflictException($"'{request.Name}' adında başka bir rol zaten mevcut.");
            }
        }
        
        var nameUpdateResult = roleToUpdate.UpdateName(request.Name);
        if (!nameUpdateResult.IsSuccess)
        {
            _logger.LogError("{RoleId} ID'li rolün adı güncellenirken validasyon hatası: {Error}", request.Id, nameUpdateResult.Error);
            throw new ValidationException(nameUpdateResult.Error);
        }

        var descriptionUpdateResult = roleToUpdate.UpdateDescription(request.Description /*?? roleToUpdate.Description*/);
        if (!descriptionUpdateResult.IsSuccess)
        {
            _logger.LogError("{RoleId} ID'li rolün açıklaması güncellenirken validasyon hatası: {Error}", request.Id, descriptionUpdateResult.Error);
            throw new ValidationException(descriptionUpdateResult.Error);
        }

        _roleRepository.Update(roleToUpdate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("{RoleId} ID'li rol başarıyla güncellendi.", request.Id);

        return true;
    }
} 