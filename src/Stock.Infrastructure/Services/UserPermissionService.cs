using Microsoft.EntityFrameworkCore;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.Application.Constants;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications;
using Stock.Domain.Specifications.Users;
using Stock.Domain.Specifications.UserPermissions;

namespace Stock.Infrastructure.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerManager<UserPermissionService> _logger;
        private readonly IPermissionService _permissionService;

        public UserPermissionService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILoggerManager<UserPermissionService> logger,
            IPermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
            _permissionService = permissionService;
        }

        public async Task<List<PermissionDto>> GetUserPermissionsAsync(int userId)
        {
            return await _permissionService.GetPermissionsByUserIdAsync(userId);
        }

        public async Task<bool> AssignPermissionToUserAsync(int userId, int permissionId, bool isGranted)
        {
            return await _permissionService.AssignPermissionToUserAsync(userId, permissionId, isGranted);
        }

        public async Task<bool> RemoveUserPermissionAsync(int userId, int permissionId)
        {
            return await _permissionService.RemoveUserPermissionAsync(userId, permissionId);
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            return await _permissionService.UserHasPermissionAsync(userId, permissionName);
        }

        public async Task<List<PermissionDto>> GetUserCustomPermissionsAsync(int userId)
        {
            try
            {
                var userPermissionRepo = _unitOfWork.GetRepository<UserPermission>();
                var spec = new UserCustomPermissionsSpecification(userId);
                var userPermissions = await userPermissionRepo.ListAsync(spec);

                return userPermissions.Select(up => new PermissionDto
                {
                    Id = up.PermissionId,
                    IsGranted = up.IsGranted,
                    Name = up.Permission?.Name ?? string.Empty,
                    ResourceType = up.Permission?.ResourceType ?? string.Empty,
                    Description = up.Permission?.Description ?? string.Empty,
                    Group = up.Permission?.Group ?? string.Empty,
                    IsCustom = true
                }).OrderBy(p => p.Group).ThenBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorGettingCustomUserPermissions, userId);
                return new List<PermissionDto>();
            }
        }

        public async Task<bool> ResetUserToRolePermissionsAsync(int userId)
        {
            return await _permissionService.ResetUserToRolePermissionsAsync(userId);
        }
    }
} 