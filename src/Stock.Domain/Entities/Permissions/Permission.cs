using System;
using System.Collections.Generic;
using Stock.Domain.Common;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities.Permissions
{
    public class Permission : BaseEntity
    {
        // Private setter'lar ile encapsulation sağlanır
        public PermissionName Name { get; private set; }
        public string Description { get; private set; }
        public string ResourceType { get; private set; }
        public string ResourceName { get; private set; }
        public string Action { get; private set; }
        public string Group { get; private set; }

        // Navigation properties
        public ICollection<RolePermission> RolePermissions { get; private set; }
        public ICollection<UserPermission> UserPermissions { get; private set; }

        // EF Core için protected constructor
        protected Permission() 
        {
            Name = null!;
            Description = null!;
            ResourceType = null!;
            ResourceName = null!;
            Action = null!;
            Group = null!;
            RolePermissions = null!;
            UserPermissions = null!;
        }

        // Primary constructor - private, sadece factory metodu üzerinden erişim
        private Permission(PermissionName name, string description, string resourceType, string resourceName, string action, string group)
        {
            Name = name;
            Description = description;
            ResourceType = resourceType;
            ResourceName = resourceName;
            Action = action;
            Group = group;
            RolePermissions = new List<RolePermission>();
            UserPermissions = new List<UserPermission>();
        }

        // Factory metodu - yeni Permission oluşturur
        public static Result<Permission> Create(
            string name,
            string description = "",
            string resourceType = "",
            string resourceName = "",
            string action = "",
            string group = "")
        {
            var permissionNameResult = PermissionName.Create(name);
            if (permissionNameResult.IsFailure)
            {
                return Result<Permission>.Failure(permissionNameResult.Error);
            }

            if (description?.Length > 200)
            {
                return Result<Permission>.Failure(DomainErrors.Permission.DescriptionTooLong);
            }

            if (resourceType?.Length > 50)
            {
                return Result<Permission>.Failure(DomainErrors.Permission.ResourceTypeTooLong);
            }

            if (group?.Length > 50)
            {
                return Result<Permission>.Failure(DomainErrors.Permission.GroupTooLong);
            }

            var permission = new Permission(
                permissionNameResult.Value,
                description ?? string.Empty,
                resourceType ?? string.Empty,
                resourceName ?? string.Empty,
                action ?? string.Empty,
                group ?? string.Empty);

            return Result<Permission>.Success(permission);
        }

        // Davranış metotları
        public Result UpdateName(string newName)
        {
            var permissionNameResult = PermissionName.Create(newName);
            if (permissionNameResult.IsFailure)
            {
                return Result.Failure(permissionNameResult.Error);
            }
            Name = permissionNameResult.Value;
            return Result.Success();
        }

        public Result UpdateDescription(string newDescription)
        {
            if (newDescription?.Length > 200)
            {
                return Result.Failure(DomainErrors.Permission.DescriptionTooLong);
            }

            Description = newDescription ?? string.Empty;
            return Result.Success();
        }

        public Result UpdateGroup(string newGroup)
        {
            if (newGroup?.Length > 50)
            {
                return Result.Failure(DomainErrors.Permission.GroupTooLong);
            }

            Group = newGroup ?? string.Empty;
            return Result.Success();
        }

        public Result UpdateResourceType(string newResourceType)
        {
            if (newResourceType?.Length > 50)
            {
                return Result.Failure(DomainErrors.Permission.ResourceTypeTooLong);
            }

            ResourceType = newResourceType ?? string.Empty;
            return Result.Success();
        }

        public void UpdateResourceName(string newResourceName)
        {
            ResourceName = newResourceName ?? string.Empty;
        }

        public void UpdateAction(string newAction)
        {
            Action = newAction ?? string.Empty;
        }
    }
} 