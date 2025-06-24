# Sonraki Adımlar

## Infrastructure Katmanı Düzeltmeleri

### 1. AuthService Sınıfı Düzeltmesi

IAuthService arayüzüne uygun olarak GenerateJwtToken metodu eklenecek:

```csharp
public string GenerateJwtToken(UserDto user)
{
    return _tokenGenerator.GenerateToken(new User
    {
        Id = user.Id,
        Username = user.Username,
        IsAdmin = user.IsAdmin,
        RoleId = user.RoleId,
        Role = user.RoleName != null ? new Role { Id = user.RoleId ?? 0, Name = user.RoleName } : null
    });
}
```

### 2. GenericRepository Sınıfı Düzeltmesi

AddAsync metodunun dönüş tipi Task olarak değiştirilecek ve SaveChangesAsync metodu eklenecek:

```csharp
public async Task AddAsync(T entity)
{
    await _dbSet.AddAsync(entity);
}

public async Task SaveChangesAsync()
{
    await _context.SaveChangesAsync();
}
```

### 3. UnitOfWork Sınıfı Düzeltmesi

CommitTransactionAsync ve RollbackTransactionAsync metotları eklenecek:

```csharp
public async Task CommitTransactionAsync()
{
    await _context.Database.CommitTransactionAsync();
}

public async Task RollbackTransactionAsync()
{
    await _context.Database.RollbackTransactionAsync();
}
```

Users ve Roles özelliklerinin tipleri düzeltilecek:

```csharp
private readonly IUserRepository _users;
private readonly IRoleRepository _roles;

public IUserRepository Users => _users;
public IRoleRepository Roles => _roles;
```

## Migrations Oluşturma

```bash
cd src/Stock.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Stock.API/Stock.API.csproj
dotnet ef database update --startup-project ../Stock.API/Stock.API.csproj
```

## API Katmanı Testi

1. Swagger UI üzerinden API endpointlerini test etme
2. Postman veya benzeri bir araç ile API endpointlerini test etme
3. JWT token oluşturma ve kullanma testi
4. Yetkilendirme kontrollerini test etme

## Frontend Adaptasyonu

1. API endpoint URL'lerini güncelleme
2. Service katmanını güncelleme
3. JWT token yönetimini güncelleme
4. Hata yönetimini güncelleme

## Performans İyileştirmeleri

1. Entity Framework Core sorgu optimizasyonu
2. N+1 sorgu problemlerini çözme
3. Önbellek mekanizması ekleme
4. API yanıt sürelerini ölçme ve iyileştirme

## Güvenlik İyileştirmeleri

1. JWT token yapılandırmasını gözden geçirme
2. HTTPS zorunlu hale getirme
3. CORS politikasını gözden geçirme
4. Rate limiting ekleme
5. SQL injection ve XSS koruması sağlama

## Dokümantasyon

1. API dokümantasyonunu tamamlama
2. Kod dokümantasyonunu tamamlama
3. Kullanım kılavuzu oluşturma
4. Deployment kılavuzu oluşturma 