using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities
{
    public class Role : BaseEntity
    {
        public RoleName Name { get; private set; }

        [StringLength(200)]
        public string Description { get; private set; } = string.Empty;

        public virtual ICollection<User> Users { get; private set; } = new HashSet<User>();
        
        public virtual ICollection<RolePermission> RolePermissions { get; private set; } = new HashSet<RolePermission>();

        // Entity Framework için protected constructor
        protected Role()
        {
            Name = null!;
            Users = new HashSet<User>();
            RolePermissions = new HashSet<RolePermission>();
        }

        // Private constructor ile nesne oluşturma kontrolü
        private Role(RoleName name, string description)
        {
            Name = name;
            Description = description;
            Users = new HashSet<User>();
            RolePermissions = new HashSet<RolePermission>();
        }

        // Factory metodu - Entity oluşturmayı kontrol ediyor
        public static Result<Role> Create(RoleName name, string description)
        {
            if (name is null)
            {
                return Result<Role>.Failure(DomainErrors.Role.RoleNameNull);
            }

            if (description != null && description.Length > 200)
            {
                return Result<Role>.Failure(DomainErrors.Role.DescriptionTooLong);
            }

            var role = new Role(name, description ?? string.Empty);
            return Result<Role>.Success(role);
        }

        // Domain davranışları - Entity'nin iş kurallarını zorunlu kılıyor
        public Result UpdateName(RoleName newName)
        {
            if (newName is null)
            {
                return Result.Failure(DomainErrors.Role.RoleNameNull);
            }

            Name = newName;
            return Result.Success();
        }

        public Result UpdateDescription(string newDescription)
        {
            if (newDescription != null && newDescription.Length > 200)
            {
                return Result.Failure(DomainErrors.Role.DescriptionTooLong);
            }

            Description = newDescription ?? string.Empty;
            return Result.Success();
        }

        // İlişki yönetimi metotları
        public void AddPermission(RolePermission permission)
        {
            RolePermissions.Add(permission);
        }

        public void RemovePermission(RolePermission permission)
        {
            RolePermissions.Remove(permission);
        }

        public void ClearPermissions()
        {
            RolePermissions.Clear();
        }
    }
} 