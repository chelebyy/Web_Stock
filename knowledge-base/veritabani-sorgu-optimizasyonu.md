# Veritabanı Sorgu Optimizasyonu

## Mevcut Durum Analizi

Stock uygulamasında veritabanı sorguları incelendiğinde aşağıdaki durumlar tespit edilmiştir:

1. **Repository Yapısı**: Uygulama, Repository Pattern kullanarak veritabanı işlemlerini soyutlamaktadır. Temel CRUD işlemleri için `Repository<T>` sınıfı, özel sorgular için ise `UserRepository`, `RoleRepository` ve `PermissionRepository` gibi özelleştirilmiş repository'ler kullanılmaktadır.

2. **Eager Loading**: Bazı sorgularda ilişkili verilerin yüklenmesi için Entity Framework Core'un `Include()` metodu kullanılmaktadır. Örneğin, `UserRepository.GetUsersWithRolesAsync()` metodu kullanıcıları rolleriyle birlikte getirmektedir.

3. **Filtreleme**: Sorguların çoğunda `IsDeleted` alanına göre filtreleme yapılmaktadır, bu da soft delete mekanizmasının kullanıldığını göstermektedir.

4. **Asenkron Operasyonlar**: Tüm veritabanı işlemleri asenkron metotlar kullanılarak gerçekleştirilmektedir, bu da uygulamanın ölçeklenebilirliğini artırmaktadır.

## Optimizasyon Önerileri

### 1. Sayfalama (Pagination) Eklenmesi

**Problem**: Tüm kullanıcıları veya rolleri getiren sorgular, veri miktarı arttıkça performans sorunlarına yol açabilir.

**Çözüm**: Sayfalama mekanizması ekleyerek, verilerin belirli sayıda ve sayfa sayfa getirilmesi sağlanabilir.

```csharp
// IUserRepository'ye eklenecek metot
Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize);

// UserRepository'deki implementasyon
public async Task<(IEnumerable<User> Users, int TotalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize)
{
    var totalCount = await _dbSet
        .Where(u => !u.IsDeleted)
        .CountAsync();
        
    var users = await _dbSet
        .Include(u => u.Role)
        .Where(u => !u.IsDeleted)
        .OrderBy(u => u.Id)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        
    return (users, totalCount);
}
```

### 2. Projection Kullanımı

**Problem**: Tüm entity'nin yüklenmesi, özellikle sadece belirli alanların gerektiği durumlarda gereksiz veri transferine neden olabilir.

**Çözüm**: Select ile projection kullanarak sadece ihtiyaç duyulan alanların getirilmesi sağlanabilir.

```csharp
// IUserRepository'ye eklenecek metot
Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync();

// UserRepository'deki implementasyon
public async Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync()
{
    return await _dbSet
        .Where(u => !u.IsDeleted)
        .Select(u => new UserSummaryDto
        {
            Id = u.Id,
            Username = u.Username,
            RoleName = u.Role.Name
        })
        .ToListAsync();
}
```

### 3. Filtreleme Optimizasyonu

**Problem**: Soft delete filtrelemesi her sorguda tekrarlanmaktadır.

**Çözüm**: Global sorgu filtreleri kullanarak, soft delete filtrelemesinin otomatik olarak uygulanması sağlanabilir.

```csharp
// ApplicationDbContext'e eklenecek
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Global sorgu filtresi
    modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
    modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
    // Diğer entity'ler için de benzer filtreler eklenebilir
    
    // Diğer konfigürasyonlar...
}
```

### 4. İndeksleme Stratejisi

**Problem**: Sık kullanılan sorgularda indeks olmaması performans sorunlarına yol açabilir.

**Çözüm**: Sık sorgulanan alanlara indeks eklenmesi.

```csharp
// Entity konfigürasyonlarında
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Mevcut konfigürasyonlar...
        
        // İndeksler
        builder.HasIndex(u => u.Username);
        builder.HasIndex(u => u.Email);
        builder.HasIndex(u => u.IsDeleted);
    }
}
```

### 5. Lazy Loading Yerine Explicit Loading

**Problem**: Lazy loading, N+1 sorgu problemine yol açabilir.

**Çözüm**: Lazy loading yerine, gerektiğinde explicit loading veya eager loading kullanılması.

```csharp
// Explicit loading örneği
public async Task<User?> GetUserWithDetailsAsync(int userId)
{
    var user = await _dbSet.FindAsync(userId);
    if (user != null && !user.IsDeleted)
    {
        await _context.Entry(user)
            .Reference(u => u.Role)
            .LoadAsync();
            
        await _context.Entry(user)
            .Collection(u => u.Permissions)
            .LoadAsync();
    }
    return user;
}
```

### 6. Önbellek (Caching) Mekanizması

**Problem**: Sık erişilen ve nadiren değişen veriler için her seferinde veritabanı sorgusu yapılması gereksiz yük oluşturabilir.

**Çözüm**: Önbellek mekanizması kullanarak, sık erişilen verilerin bellekte tutulması.

```csharp
// IUserRepository'ye eklenecek metot (önbellekli)
Task<IEnumerable<Role>> GetAllRolesCachedAsync();

// RoleRepository'deki implementasyon
public async Task<IEnumerable<Role>> GetAllRolesCachedAsync()
{
    string cacheKey = "AllRoles";
    if (!_cache.TryGetValue(cacheKey, out IEnumerable<Role> roles))
    {
        roles = await _dbSet
            .Where(r => !r.IsDeleted)
            .ToListAsync();
            
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            
        _cache.Set(cacheKey, roles, cacheOptions);
    }
    return roles;
}
```

## Uygulama Planı

Veritabanı sorgu optimizasyonları aşağıdaki sırayla uygulanacaktır:

1. **Sayfalama Mekanizması**: Öncelikle kullanıcı ve rol listeleme işlemlerine sayfalama eklenerek, büyük veri setlerinin performanslı şekilde işlenmesi sağlanacaktır.

2. **Projection Kullanımı**: Sadece gerekli alanların getirilmesi için projection kullanılacaktır.

3. **Global Sorgu Filtreleri**: Soft delete filtrelemesi için global sorgu filtreleri eklenecektir.

4. **İndeksleme**: Sık sorgulanan alanlara indeks eklenecektir.

5. **Önbellek Mekanizması**: Sık erişilen ve nadiren değişen veriler için önbellek mekanizması eklenecektir.

## Beklenen Faydalar

- **Performans İyileştirmesi**: Sorgu optimizasyonları ile veritabanı işlemlerinin daha hızlı gerçekleşmesi sağlanacaktır.
- **Kaynak Kullanımı**: Gereksiz veri transferi ve işleme azaltılarak, sistem kaynakları daha verimli kullanılacaktır.
- **Ölçeklenebilirlik**: Veri miktarı arttıkça sistemin performansının korunması sağlanacaktır.
- **Kullanıcı Deneyimi**: Daha hızlı yanıt süreleri ile kullanıcı deneyimi iyileştirilecektir.

## Sonraki Adımlar

1. Sayfalama mekanizmasının eklenmesi
2. Projection kullanımının yaygınlaştırılması
3. Global sorgu filtrelerinin eklenmesi
4. İndeksleme stratejisinin uygulanması
5. Önbellek mekanizmasının eklenmesi 