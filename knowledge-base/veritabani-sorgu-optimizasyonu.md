# Veritabanı Sorgu Optimizasyonu

**Son Güncelleme:** 01.07.2025

## İçindekiler
1. [Mevcut Durum Analizi](#mevcut-durum-analizi)
2. [Optimizasyon Önerileri](#optimizasyon-önerileri)
3. [Uygulama Planı](#uygulama-planı)
4. [Beklenen Faydalar](#beklenen-faydalar)
5. [Karşılaşılan Sorunlar ve Çözümleri](#karşılaşılan-sorunlar-ve-çözümleri)
6. [Öğrenilen Dersler](#öğrenilen-dersler)

## Mevcut Durum Analizi

Stock uygulamasında veritabanı sorguları şu anda aşağıdaki şekilde gerçekleştiriliyor:

### Repository Pattern Kullanımı
- Tüm veritabanı işlemleri için Repository Pattern kullanılıyor
- Generic Repository sınıfı temel CRUD işlemlerini sağlıyor
- Entity-spesifik repository'ler özel sorgular için kullanılıyor

### Eager Loading
- İlişkili veriler genellikle Include ile yükleniyor
- Bazı durumlarda lazy loading kullanılıyor, bu da N+1 sorununa yol açabiliyor

### Filtreleme
- Filtreleme işlemleri genellikle Where ile yapılıyor
- Bazı durumlarda filtreleme mantığı controller'larda uygulanıyor

### Asenkron Operasyonlar
- Veritabanı işlemleri için async/await kullanılıyor
- Bazı durumlarda senkron metotlar kullanılıyor, bu da performans sorunlarına yol açabiliyor

### Tespit Edilen Sorunlar
1. **N+1 Sorunu**: İlişkili veriler için çok sayıda sorgu yapılıyor
2. **Gereksiz Veri Yükleme**: Tüm alanlar her zaman yükleniyor
3. **Sayfalama Eksikliği**: Büyük veri kümeleri için sayfalama kullanılmıyor
4. **Önbellek Eksikliği**: Sık erişilen veriler için önbellek kullanılmıyor
5. **İndeks Eksikliği**: Sık filtrelenen alanlar için indeksler yok
6. **Bağlantı Yönetimi**: Veritabanı bağlantıları uzun süre açık kalabiliyor

## Optimizasyon Önerileri

### 1. Sayfalama (Pagination)
Büyük veri kümeleri için sayfalama uygulanması:

```csharp
public async Task<(IEnumerable<T> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize)
{
    // Sayfalama parametrelerini doğrula
    pageNumber = Math.Max(1, pageNumber);
    pageSize = Math.Clamp(pageSize, 1, 100); // Sayfa boyutunu 1-100 arasında sınırla
    
    var query = _dbSet.AsNoTracking();
    var totalCount = await query.CountAsync();
    
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    
    return (items, totalCount);
}
```

### 2. Projection (Select)
Sadece ihtiyaç duyulan alanların seçilmesi:

```csharp
public async Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync()
{
    return await _dbSet
        .AsNoTracking()
        .Select(u => new UserSummaryDto
        {
            Id = u.Id,
            Username = u.Username,
            Sicil = u.Sicil,
            RoleName = u.Role.Name // Navigation property üzerinden doğrudan erişim
        })
        .ToListAsync();
}
```

### 3. Global Query Filtreleri
Silinmiş kayıtların otomatik olarak filtrelenmesi:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Global query filter for soft delete
    modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
    modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
    modelBuilder.Entity<Permission>().HasQueryFilter(p => !p.IsDeleted);
    
    // Other configurations...
}
```

### 4. İndeksleme Stratejileri
Sık filtrelenen ve sıralanan alanlar için indeksler:

```csharp
// UserConfiguration.cs
public void Configure(EntityTypeBuilder<User> builder)
{
    // ... diğer konfigürasyonlar ...
    
    // Sık filtrelenen ve sıralanan alanlar için indeksler
    builder.HasIndex(u => u.Username);
    builder.HasIndex(u => u.Sicil);
    builder.HasIndex(u => new { u.IsAdmin, u.LastLoginAt }); // Bileşik indeks
}
```

### 5. Önbellek Mekanizması
Sık erişilen ve nadiren değişen veriler için önbellek:

```csharp
public async Task<IEnumerable<Role>> GetAllRolesAsync()
{
    // Önbellekte var mı kontrol et
    if (!_memoryCache.TryGetValue("AllRoles", out List<Role> roles))
    {
        // Önbellekte yoksa veritabanından çek
        roles = await _dbContext.Roles.AsNoTracking().ToListAsync();
        
        // Önbelleğe ekle (10 dakika süreyle)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
            .SetPriority(CacheItemPriority.Normal);
        
        _memoryCache.Set("AllRoles", roles, cacheOptions);
    }
    
    return roles;
}
```

## Uygulama Planı

### Aşama 1: Altyapı Hazırlığı
1. Sayfalama için gerekli DTO'ların oluşturulması
2. Önbellek servisinin eklenmesi
3. Global query filtreleri için DbContext'in güncellenmesi

### Aşama 2: Repository Sınıflarının Güncellenmesi
1. Generic Repository'ye sayfalama desteğinin eklenmesi
2. Entity-spesifik repository'lere projection desteğinin eklenmesi
3. Asenkron operasyonların tutarlı hale getirilmesi

### Aşama 3: İndeksleme ve Veritabanı Optimizasyonu
1. Entity Configuration sınıflarına indekslerin eklenmesi
2. Migration oluşturulması ve uygulanması
3. Veritabanı bağlantı ayarlarının optimize edilmesi

### Aşama 4: Controller ve Servis Katmanının Güncellenmesi
1. Controller'lara sayfalama desteğinin eklenmesi
2. Servis katmanına önbellek desteğinin eklenmesi
3. API endpoint'lerinin güncellenmesi

### Aşama 5: Test ve İzleme
1. Performans testlerinin yapılması
2. Sorgu izleme ve analiz araçlarının kurulması
3. Düzenli performans izleme planının oluşturulması

## Beklenen Faydalar

1. **Performans İyileştirmesi**: Sorgu sayısının ve veritabanı yükünün azalması
2. **Kaynak Kullanımı**: Bellek ve CPU kullanımının optimize edilmesi
3. **Ölçeklenebilirlik**: Büyük veri kümeleri için daha iyi performans
4. **Kullanıcı Deneyimi**: Daha hızlı sayfa yükleme süreleri
5. **Güvenlik**: Hassas verilerin daha iyi korunması

## Karşılaşılan Sorunlar ve Çözümleri

### Sorun 1: Sayfalama Parametrelerinin Doğrulanmaması
**Çözüm:** Tüm repository sınıflarında sayfalama parametreleri için doğrulama eklendi.

### Sorun 2: N+1 Sorunu
**Çözüm:** Include ve Select kullanılarak ilişkili verilerin tek sorguda yüklenmesi sağlandı.

### Sorun 3: Büyük Veri Kümelerinde Bellek Tüketimi
**Çözüm:** Streaming ve asenkron akışlar kullanılarak verilerin parça parça işlenmesi sağlandı.

### Sorun 4: Karmaşık Sorgularda Performans Sorunları
**Çözüm:** EXPLAIN ANALYZE kullanılarak sorgu planları analiz edildi ve indeksleme stratejileri uygulandı.

### Sorun 5: Veritabanı Bağlantı Dizesi Güvenliği
**Çözüm:** Bağlantı dizesi şifresi ortam değişkenlerinden alınarak güvenlik artırıldı.

## Öğrenilen Dersler

1. Sayfalama parametrelerini her zaman doğrulamak önemlidir
2. Include kullanarak ilişkili verileri tek sorguda yüklemek, N+1 sorununu önler
3. AsNoTracking kullanarak sadece okuma amaçlı sorgularda change tracking'i devre dışı bırakmak, performansı artırır
4. Büyük veri kümeleri için streaming ve asenkron akışlar kullanmak, bellek tüketimini azaltır
5. Sık erişilen ve nadiren değişen veriler için önbellek kullanmak, veritabanı yükünü azaltır
6. Hassas bilgileri açık metin olarak yapılandırma dosyalarında saklamaktan kaçınmak gerekir

## Sonuç

Veritabanı sorgu optimizasyonu çalışmaları, Stock uygulamasının performansını ve ölçeklenebilirliğini önemli ölçüde artırmıştır. Sayfalama, projection, global query filtreleri, indeksleme ve önbellek mekanizmaları gibi optimizasyon teknikleri uygulanarak, veritabanı sorguları daha verimli hale getirilmiştir.

Bu optimizasyonlar sayesinde, uygulama daha büyük veri kümeleriyle çalışabilir hale gelmiş ve kullanıcı deneyimi iyileştirilmiştir. Ayrıca, veritabanı bağlantı yönetimi ve güvenliği de artırılmıştır.

Gelecekte, performans metriklerinin düzenli olarak izlenmesi ve gerektiğinde ek optimizasyonların yapılması planlanmaktadır. 