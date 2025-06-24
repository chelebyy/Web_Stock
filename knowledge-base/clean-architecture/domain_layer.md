# Domain Katmanı Geçişi

## Genel Bakış

Domain katmanı, Clean Architecture'ın en iç katmanıdır ve iş mantığının merkezini oluşturur. Bu katman, dış katmanlara bağımlı olmamalı ve saf C# kodu içermelidir. Domain katmanı, entity'leri, value object'leri, domain service'leri ve repository interface'lerini içerir.

## Taşınacak Bileşenler

1. **Entity'ler**
   - User
   - Role
   - AuditLog
   - Diğer domain entity'leri

2. **Repository Interface'leri**
   - IRepository<T>
   - IUserRepository
   - IRoleRepository
   - Diğer özel repository'ler

3. **Domain Service Interface'leri**
   - IPasswordHasher
   - Diğer domain servis interface'leri

4. **Value Objects** (gerekirse)
   - Email
   - Password
   - Diğer value object'ler

## Geçiş Adımları

### 1. BaseEntity Sınıfı

```csharp
namespace Stock.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
```

### 2. Entity Sınıfları

```csharp
namespace Stock.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }

    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }

    public class AuditLog : BaseEntity
    {
        public string Action { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public virtual User? User { get; set; }
    }
}
```

### 3. Repository Interface'leri

```csharp
namespace Stock.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
    }

    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersWithRolesAsync();
    }

    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetRolesWithUsersAsync();
    }
}
```

### 4. Domain Service Interface'leri

```csharp
namespace Stock.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
```

### 5. UnitOfWork Interface'i

```csharp
namespace Stock.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
```

## Dikkat Edilecek Noktalar

1. **Bağımlılık Yönetimi**
   - Domain katmanı dış katmanlara bağımlı olmamalı
   - Infrastructure veya Application katmanlarından referans almamalı
   - Sadece .NET Core kütüphanelerine bağımlı olmalı

2. **Entity İlişkileri**
   - Navigation property'ler virtual olarak işaretlenmeli
   - Collection'lar için HashSet<T> kullanılmalı
   - Foreign key'ler nullable olabilir
   - Constructor'da collection'lar initialize edilmeli

3. **Validation**
   - Domain entity'lerde temel validation kuralları uygulanabilir
   - Karmaşık validation kuralları Application katmanına bırakılmalı

4. **Naming Conventions**
   - Interface'ler "I" ile başlamalı
   - Repository'ler "Repository" ile bitmeli
   - Entity'ler tekil isimlendirilmeli

## Sonraki Adımlar

Domain katmanı tamamlandıktan sonra:

1. Application katmanı için DTO'lar oluşturulacak
2. AutoMapper profilleri tanımlanacak
3. CQRS yapısı kurulacak
4. Validation kuralları tanımlanacak 