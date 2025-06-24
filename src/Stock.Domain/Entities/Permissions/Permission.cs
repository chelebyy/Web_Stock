using System;
using System.Collections.Generic;
using Stock.Domain.Common;
using Stock.Domain.Entities;

namespace Stock.Domain.Entities.Permissions
{
    public class Permission : BaseEntity
    {
        // Private setter'lar ile encapsulation sağlanır
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ResourceType { get; private set; }
        public string ResourceName { get; private set; }
        public string Action { get; private set; }
        public string Group { get; private set; }

        // Navigation properties
        public ICollection<RolePermission> RolePermissions { get; private set; }
        public ICollection<UserPermission> UserPermissions { get; private set; }

        // EF Core için protected constructor
        protected Permission() { }

        // Primary constructor - private, sadece factory metodu üzerinden erişim
        private Permission(string name, string description, string resourceType, string resourceName, string action, string group)
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
            // İş kuralları ve doğrulama kontrolleri
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<Permission>.Failure(DomainErrors.Permission.NameEmpty);
            }

            if (name.Length > 100)
            {
                return Result<Permission>.Failure(DomainErrors.Permission.NameTooLong);
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
                name,
                description ?? string.Empty,
                resourceType ?? string.Empty,
                resourceName ?? string.Empty,
                action ?? string.Empty,
                group ?? string.Empty);

            return Result<Permission>.Success(permission);
        }

        // Davranış metotları
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