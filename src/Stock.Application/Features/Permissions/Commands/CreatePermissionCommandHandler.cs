using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Models.DTOs;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Commands
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, Result<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePermissionCommandHandler> _logger;

        public CreatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CreatePermissionCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<PermissionDto>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni izin oluşturuluyor: {PermissionName}", request.Name);

            // Permission entity'sini oluşturmak için factory metodunu kullan
            var createResult = Permission.Create(
                request.Name,
                request.Description,
                request.ResourceType,
                request.ResourceName,
                request.Action,
                request.Group);

            if (!createResult.IsSuccess)
            {
                _logger.LogWarning("İzin oluşturma başarısız (Domain Validation): {Error}", createResult.Error);
                return Result<PermissionDto>.Failure(createResult.Error);
            }

            var newPermission = createResult.Value;

            try
            {
                var repository = _unitOfWork.GetRepository<Permission>();
                await repository.AddAsync(newPermission, cancellationToken);

                // Değişiklikleri kaydet (IUnitOfWork üzerinden)
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("İzin oluşturulurken veritabanına kaydetme başarısız.");
                    return Result<PermissionDto>.Failure("Failed to save the new permission to the database.");
                }

                _logger.LogInformation("İzin başarıyla oluşturuldu: ID {PermissionId}, Name: {PermissionName}", newPermission.Id, newPermission.Name);

                var permissionDto = _mapper.Map<PermissionDto>(newPermission);
                return Result<PermissionDto>.Success(permissionDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "İzin oluşturulurken bir hata oluştu: {PermissionName}", request.Name);
                return Result<PermissionDto>.Failure($"An error occurred while creating the permission: {ex.Message}");
            }
        }
    }
} 