# Önbellek (Cache) Mekanizması

## Genel Bakış

Önbellek mekanizması, uygulamanın performansını artırmak ve veritabanı yükünü azaltmak için sık kullanılan verileri geçici olarak bellekte saklayan bir sistemdir. Bu belge, Stock uygulamasında uygulanan önbellek mekanizmasının detaylarını açıklamaktadır.

## Bileşenler

### 1. ICacheService Arayüzü

`ICacheService` arayüzü, önbellek işlemlerini standartlaştırmak için kullanılır. Bu arayüz, aşağıdaki temel işlemleri tanımlar:

- Veri alma (`Get<T>`, `GetAsync<T>`)
- Veri ekleme (`Set<T>`, `SetAsync<T>`)
- Veri silme (`Remove`, `RemoveAsync`)
- Veri varlığı kontrolü (`Exists`, `ExistsAsync`)
- Önbellek temizleme (`Clear`, `ClearAsync`)
- Veri alma veya yoksa ekleme (`GetOrSet<T>`, `GetOrSetAsync<T>`)

### 2. MemoryCacheService Sınıfı

`MemoryCacheService` sınıfı, .NET'in `IMemoryCache` arayüzünü kullanarak `ICacheService` arayüzünü uygular. Bu sınıf, verileri uygulama belleğinde saklar ve belirtilen süre sonunda otomatik olarak temizler.

### 3. Repository Sınıflarında Önbellek Kullanımı

Repository sınıfları (UserRepository, RoleRepository, PermissionRepository), veritabanı sorgularını önbellekte saklamak için `ICacheService`'i kullanır. Bu, aşağıdaki avantajları sağlar:

- Tekrarlanan sorguların veritabanına gitmeden önbellekten alınması
- Veritabanı yükünün azaltılması
- Yanıt sürelerinin iyileştirilmesi

## Önbellek Stratejisi

### 1. Önbellek Anahtarları

Her repository sınıfı, önbellek anahtarlarını tanımlamak için sabit değişkenler kullanır:

```csharp
private const string CACHE_KEY_ALL_USERS = "ALL_USERS";
private const string CACHE_KEY_USER_BY_ID = "USER_BY_ID_";
private const string CACHE_KEY_USER_BY_USERNAME = "USER_BY_USERNAME_";
```

### 2. Önbellek Süresi

Veriler, veri türüne göre farklı sürelerde önbellekte tutulur:

- Kullanıcılar: 30 dakika
- Roller: 60 dakika
- İzinler: 120 dakika

Bu süreler, verilerin değişim sıklığına göre belirlenmiştir. Örneğin, izinler nadiren değiştiği için daha uzun süre önbellekte tutulur.

### 3. Önbellek İnvalidasyonu

Veri değişikliklerinde önbelleğin güncel kalması için, ilgili önbellek girişleri otomatik olarak temizlenir:

```csharp
private async Task InvalidateUserCacheAsync(int? userId = null)
{
    _logger.LogInfo("Kullanıcı önbelleği temizleniyor.");
    
    // Tüm kullanıcılar önbelleğini temizle
    await _cacheService.RemoveAsync(CACHE_KEY_ALL_USERS);
    
    // Kullanıcı özetleri önbelleğini temizle
    await _cacheService.RemoveAsync(CACHE_KEY_USER_SUMMARIES);
    
    // Belirli bir kullanıcı ID'si verilmişse, o kullanıcıya özgü önbelleği temizle
    if (userId.HasValue)
    {
        await _cacheService.RemoveAsync($"{CACHE_KEY_USER_BY_ID}{userId.Value}");
    }
}
```

## Önbellek Kullanım Örnekleri

### 1. Veri Getirme

```csharp
public async Task<User> GetByIdAsync(int id)
{
    string cacheKey = $"{CACHE_KEY_USER_BY_ID}{id}";
    
    return await _cacheService.GetOrSetAsync(cacheKey, async () => 
    {
        _logger.LogInfo($"Veritabanından kullanıcı getiriliyor. ID: {id}");
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }, CACHE_DURATION_MINUTES);
}
```

### 2. Sayfalanmış Veri Getirme

```csharp
public async Task<(IEnumerable<User> users, int totalCount)> GetPaginatedUsersAsync(int pageNumber, int pageSize, string searchTerm = null, string sortBy = null, bool ascending = true)
{
    string cacheKey = $"{CACHE_KEY_PAGINATED_USERS}page{pageNumber}_size{pageSize}_search{searchTerm ?? "none"}_sort{sortBy ?? "none"}_{ascending}";
    
    return await _cacheService.GetOrSetAsync(cacheKey, async () => 
    {
        // Veritabanı sorgusu...
    }, CACHE_DURATION_MINUTES);
}
```

### 3. Veri Değişikliği ve Önbellek İnvalidasyonu

```csharp
public override async Task<User> AddAsync(User entity)
{
    var result = await base.AddAsync(entity);
    await InvalidateUserCacheAsync();
    return result;
}

public override async Task UpdateAsync(User entity)
{
    await base.UpdateAsync(entity);
    await InvalidateUserCacheAsync(entity.Id);
}
```

## Performans İyileştirmeleri

Önbellek mekanizması, aşağıdaki performans iyileştirmelerini sağlar:

1. **Veritabanı Yükünün Azaltılması**: Sık kullanılan veriler önbellekten alınarak veritabanı yükü azaltılır.
2. **Yanıt Sürelerinin İyileştirilmesi**: Önbellekten veri alma, veritabanından veri almaya göre çok daha hızlıdır.
3. **Ölçeklenebilirlik**: Veritabanı yükünün azaltılması, uygulamanın daha fazla kullanıcıya hizmet verebilmesini sağlar.

## Dikkat Edilmesi Gerekenler

1. **Önbellek Tutarlılığı**: Veri değişikliklerinde ilgili önbellek girişlerinin temizlenmesi önemlidir.
2. **Önbellek Boyutu**: Çok fazla veri önbellekte tutulursa, bellek tüketimi artabilir.
3. **Önbellek Süresi**: Verilerin değişim sıklığına göre uygun önbellek süresi belirlenmelidir.

## Gelecek İyileştirmeler

1. **Dağıtık Önbellek**: Uygulamanın ölçeklenebilirliğini artırmak için Redis gibi dağıtık önbellek çözümleri kullanılabilir.
2. **Önbellek İstatistikleri**: Önbellek kullanımını izlemek için istatistikler toplanabilir.
3. **Önbellek Politikaları**: Farklı veri türleri için farklı önbellek politikaları uygulanabilir.

## Sonuç

Önbellek mekanizması, Stock uygulamasının performansını ve ölçeklenebilirliğini önemli ölçüde artırmıştır. Veritabanı yükünün azaltılması ve yanıt sürelerinin iyileştirilmesi, kullanıcı deneyimini olumlu yönde etkilemiştir. 