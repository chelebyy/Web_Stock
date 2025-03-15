# Veritabanı Sorgu Optimizasyonu

Bu belge, Stock projesinde veritabanı sorgularının optimizasyonu için yapılan değişiklikleri ve kullanılan teknikleri açıklamaktadır.

## Yapılan Değişiklikler

### 1. AsNoTracking Kullanımı

Entity Framework Core'da `AsNoTracking()` metodu, veritabanından alınan varlıkların değişiklik izleme mekanizması tarafından takip edilmemesini sağlar. Bu, salt okunur sorgular için performansı önemli ölçüde artırır.

```csharp
var users = await _dbSet
    .AsNoTracking() // Performans için tracking'i devre dışı bırak
    .Include(u => u.Role)
    .ToListAsync();
```

### 2. Projection Optimizasyonu

Tüm varlık yerine sadece ihtiyaç duyulan alanları seçerek bellek kullanımını ve veri transferini azaltır.

```csharp
return await _dbSet
    .AsNoTracking()
    .Include(u => u.Role)
    .Select(u => new User
    {
        Id = u.Id,
        Username = u.Username,
        Sicil = u.Sicil,
        IsAdmin = u.IsAdmin,
        Role = new Role { Id = u.Role.Id, Name = u.Role.Name }
    })
    .ToListAsync();
```

### 3. Filtreleme ve Sıralama

Dinamik filtreleme ve sıralama için IQueryable üzerinde çalışarak, veritabanı sorgusunun en verimli şekilde oluşturulmasını sağlar.

```csharp
// Filtreleme için IQueryable oluştur
var query = _dbSet.AsNoTracking().Include(u => u.Role).AsQueryable();

// Filtreleri uygula
if (!string.IsNullOrWhiteSpace(usernameFilter))
    query = query.Where(u => u.Username.Contains(usernameFilter));
    
// Sıralama uygula
query = ApplySorting(query, sortBy, sortAscending);
```

### 4. Sayfalama

Büyük veri setleri için sayfalama kullanarak, sadece gerekli verilerin getirilmesini sağlar.

```csharp
var users = await query
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### 5. İndeksleme

Sık sorgulanan alanlar için veritabanı indeksleri oluşturarak sorgu performansını artırır.

```csharp
// UserConfiguration.cs
builder.HasIndex(u => u.Username).IsUnique();
builder.HasIndex(u => u.Sicil);
builder.HasIndex(u => u.IsDeleted);
builder.HasIndex(u => u.RoleId);
```

### 6. Performans İzleme

Sorgu performansını izlemek için `Stopwatch` kullanarak, sorgu sürelerini ölçer ve loglama yapar.

```csharp
var stopwatch = Stopwatch.StartNew();
// Sorgu işlemleri
stopwatch.Stop();
var elapsedMs = stopwatch.ElapsedMilliseconds;
_logger.LogInfo($"Sorgu {elapsedMs}ms sürede tamamlandı.");
```

## Faydalar

1. **Daha Hızlı Yanıt Süreleri**: Optimize edilmiş sorgular, kullanıcılara daha hızlı yanıt süresi sağlar.
2. **Azaltılmış Sunucu Yükü**: Veritabanı sunucusu üzerindeki yük azalır, daha fazla eşzamanlı istek işlenebilir.
3. **Daha Az Bellek Kullanımı**: Sadece gerekli verilerin getirilmesi, uygulama bellek kullanımını azaltır.
4. **Ölçeklenebilirlik**: Büyük veri setleri için bile uygulamanın performanslı çalışmasını sağlar.
5. **İzlenebilirlik**: Performans ölçümleri sayesinde, sorunlu sorgular tespit edilebilir ve iyileştirilebilir.

## En İyi Uygulamalar

1. Salt okunur sorgular için her zaman `AsNoTracking()` kullanın.
2. Sadece ihtiyaç duyulan alanları seçin (projection).
3. Büyük veri setleri için sayfalama kullanın.
4. Sık sorgulanan alanlar için indeksler oluşturun.
5. Sorgu performansını düzenli olarak izleyin ve optimize edin.
6. Karmaşık sorgular için stored procedure kullanmayı değerlendirin.
7. N+1 sorgu probleminden kaçınmak için Include ve ThenInclude kullanın.

## Örnek Kullanım

```csharp
// Controller
[HttpGet("paginated")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> GetPaginated(
    [FromQuery] int pageNumber = 1, 
    [FromQuery] int pageSize = 10,
    [FromQuery] string usernameFilter = null,
    [FromQuery] string sortBy = "Username",
    [FromQuery] bool sortAscending = true)
{
    var query = new GetPaginatedUsersQuery
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        UsernameFilter = usernameFilter,
        SortBy = sortBy,
        SortAscending = sortAscending
    };
    
    var result = await _mediator.Send(query);
    return Ok(result);
}
```

## Sonuç

Veritabanı sorgu optimizasyonu, uygulamanın performansını ve ölçeklenebilirliğini önemli ölçüde artırır. Bu belgede açıklanan teknikleri kullanarak, Stock projesindeki veritabanı sorguları optimize edilmiş ve daha iyi bir kullanıcı deneyimi sağlanmıştır. 