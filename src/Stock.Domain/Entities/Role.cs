using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Stock.Domain.Common;
using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; private set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; private set; } = string.Empty;

        public virtual ICollection<User> Users { get; private set; } = new HashSet<User>();
        
        public virtual ICollection<RolePermission> RolePermissions { get; private set; } = new HashSet<RolePermission>();

        // Entity Framework için protected constructor
        protected Role()
        {
            Users = new HashSet<User>();
            RolePermissions = new HashSet<RolePermission>();
        }

        // Private constructor ile nesne oluşturma kontrolü
        private Role(string name, string description)
        {
            Name = name;
            Description = description;
            Users = new HashSet<User>();
            RolePermissions = new HashSet<RolePermission>();
        }

        // Factory metodu - Entity oluşturmayı kontrol ediyor
        public static Result<Role> Create(string name, string description)
        {
            // Validasyon kuralları
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<Role>.Failure(DomainErrors.Role.NameEmpty);
            }

            if (name.Length > 50)
            {
                return Result<Role>.Failure(DomainErrors.Role.NameTooLong);
            }

            if (description != null && description.Length > 200)
            {
                return Result<Role>.Failure(DomainErrors.Role.DescriptionTooLong);
            }

            var role = new Role(name, description ?? string.Empty);
            return Result<Role>.Success(role);
        }

        // Domain davranışları - Entity'nin iş kurallarını zorunlu kılıyor
        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                return Result.Failure(DomainErrors.Role.NameEmpty);
            }

            if (newName.Length > 50)
            {
                return Result.Failure(DomainErrors.Role.NameTooLong);
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