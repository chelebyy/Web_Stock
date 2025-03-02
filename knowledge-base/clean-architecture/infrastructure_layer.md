# Infrastructure Katmanı Geçişi

## Genel Bakış

Infrastructure katmanı, Clean Architecture'ın dış katmanlarından biridir ve veritabanı erişimi, harici servislerle iletişim, dosya sistemi erişimi gibi teknik detayları içerir. Bu katman, Application katmanında tanımlanan arayüzlerin uygulamalarını sağlar ve Domain katmanına bağımlıdır.

## Taşınacak Bileşenler

1. **DbContext**
   - ApplicationDbContext
   - Entity konfigürasyonları

2. **Repository Uygulamaları**
   - Repository<T>
   - UserRepository
   - RoleRepository
   - Diğer özel repository'ler

3. **UnitOfWork Uygulaması**
   - UnitOfWork

4. **Harici Servis Entegrasyonları**
   - PasswordHasher
   - JwtTokenGenerator
   - Diğer servisler

5. **Migrations**
   - Mevcut migration'lar

## Geçiş Adımları

### 1. NuGet Paketleri

Infrastructure katmanı için gerekli NuGet paketlerini ekleyelim:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt
```

### 2. DbContext Yapılandırması

```csharp
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data.Configurations;

namespace Stock.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AuditLogConfiguration());

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin role
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Administrator role with full access",
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );

            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAELTKkUzKvFLVNEPNpQnNx8O1BdYeV9dQrP+BIrYnKO4GVF8SYMk7rHQH9MQQkp8Jrw==", // "admin123"
                    IsAdmin = true,
                    RoleId = 1,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
        }
    }
}
```

### 3. Entity Konfigürasyonları

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.IsAdmin)
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .HasMaxLength(200);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false);
        }
    }

    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.UserId)
                .HasMaxLength(50);

            builder.Property(a => a.Path)
                .HasMaxLength(200);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
```

### 4. Repository Uygulamaları

```csharp
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && !u.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            return await _dbSet
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }
    }

    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            return await _dbSet
                .Include(r => r.Users)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }
    }
}
```

### 5. UnitOfWork Uygulaması

```csharp
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Stock.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        public IRoleRepository Roles => _roleRepository ??= new RoleRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
```

### 6. Harici Servis Entegrasyonları

```csharp
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Stock.Domain.Interfaces;
using System;
using System.Security.Cryptography;

namespace Stock.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Format: {algorithm}${iterations}${salt}${hash}
            return $"PBKDF2${10000}${Convert.ToBase64String(salt)}${hashed}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            // Parse the hash
            var parts = passwordHash.Split('$');
            if (parts.Length != 4)
            {
                return false;
            }

            var algorithm = parts[0];
            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var hash = parts[3];

            // Verify the hash
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: 256 / 8));

            return hash == hashed;
        }
    }
}
```

### 7. Dependency Injection

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Repositories;
using Stock.Infrastructure.Services;
using Stock.Infrastructure.UnitOfWork;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Services
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
```

### 8. Migrations Taşıma

Mevcut migration'ları taşımak için aşağıdaki adımları izleyebiliriz:

1. Önce yeni bir migration oluşturalım:

```bash
cd src/Stock.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Stock.API
```

2. Mevcut veritabanı şemasını kullanmak için migration'ı düzenleyelim.

3. Veritabanını güncelleyelim:

```bash
dotnet ef database update --startup-project ../Stock.API
```

## Dikkat Edilecek Noktalar

1. **Bağımlılık Yönetimi**
   - Infrastructure katmanı Domain ve Application katmanlarına bağımlı olabilir
   - API katmanı Infrastructure katmanına bağımlı olmalı

2. **Entity Framework Konfigürasyonları**
   - Entity konfigürasyonları ayrı dosyalarda tutulmalı
   - Fluent API kullanılarak ilişkiler tanımlanmalı
   - Seed data eklenmeli

3. **Repository Uygulamaları**
   - Generic repository pattern kullanılmalı
   - Özel repository'ler için ek metodlar eklenebilir
   - Soft delete kullanılmalı

4. **UnitOfWork**
   - Transaction yönetimi için UnitOfWork kullanılmalı
   - Repository'ler UnitOfWork üzerinden erişilmeli

## Sonraki Adımlar

Infrastructure katmanı tamamlandıktan sonra:

1. API katmanı için controller'lar oluşturulacak
2. Middleware'ler taşınacak
3. Program.cs ve Startup yapılandırması taşınacak
4. Swagger/OpenAPI yapılandırması taşınacak
5. Authentication/Authorization yapılandırması taşınacak 