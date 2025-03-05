# Backend Model Uyumsuzluğu ve Migration Sorunları

## Genel Bakış

Bu belge, backend uygulamasında karşılaşılan model uyumsuzluğu ve Entity Framework Core migration sorunlarını ve bunların çözümlerini detaylandırmaktadır. Özellikle, UserPermissionService'deki büyük/küçük harf duyarlılığı sorunu ve migration'lar arasındaki çakışmalar ele alınmıştır.

## Karşılaşılan Sorunlar

### 1. Büyük/Küçük Harf Duyarlılığı Sorunu

**Hata Mesajı:**
```
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\Services\UserPermissionService.cs(208,44): error CS1061: 'User' bir 'UserName' tanımı içermiyor
```

**Nedeni:**
User entity sınıfında property adı `Username` olarak tanımlanmışken, UserPermissionService sınıfında bu property'ye `UserName` olarak erişilmeye çalışılıyordu. C# büyük/küçük harf duyarlı bir dil olduğundan, bu durum derleme hatasına neden oldu.

**User Entity Tanımı:**
```csharp
public class User : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    // Diğer property'ler...
}
```

**Hatalı Kullanım:**
```csharp
.Select(up => new UserPermissionDto
{
    // ... diğer property'ler
    UserName = up.User.UserName, // Hata: UserName yerine Username olmalı
    // ... diğer property'ler
})
```

### 2. Migration Çakışmaları

**Hata Mesajı 1:**
```
42701: column "Action" of relation "Permissions" already exists
```

**Hata Mesajı 2:**
```
42704: constraint "FK_AuditLogs_Users_UserId" of relation "AuditLogs" does not exist
```

**Hata Mesajı 3:**
```
The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database.
```

**Nedeni:**
Farklı zamanlarda oluşturulmuş migration'lar arasında çakışmalar vardı. Özellikle:
1. Bir migration, zaten var olan bir sütunu tekrar eklemeye çalışıyordu.
2. Başka bir migration, var olmayan bir kısıtlamayı kaldırmaya çalışıyordu.
3. Model değişiklikleri yapılmış ancak yeni bir migration oluşturulmamıştı.

## Uygulanan Çözümler

### 1. Büyük/Küçük Harf Duyarlılığı Düzeltmesi

UserPermissionService.cs dosyasında hatalı olan 'UserName' referansı 'Username' olarak düzeltildi:

```csharp
// Düzeltilmiş kod
.Select(up => new UserPermissionDto
{
    // ... diğer property'ler
    UserName = up.User.Username, // UserName -> Username olarak düzeltildi
    // ... diğer property'ler
})
```

### 2. Migration Sorunlarının Çözümü

Migration sorunlarını çözmek için radikal bir yaklaşım uygulandı:

1. **Tüm Migration'ların Silinmesi:**
   ```powershell
   rm -r -force Migrations
   ```

2. **Veritabanının Tamamen Silinmesi:**
   ```powershell
   dotnet ef database drop -p ../Stock.Infrastructure -f
   ```

3. **Yeni Bir Initial Migration Oluşturulması:**
   ```powershell
   dotnet ef migrations add InitialMigration -p ../Stock.Infrastructure
   ```

4. **Yeni Migration'ın Uygulanması:**
   ```powershell
   dotnet ef database update -p ../Stock.Infrastructure
   ```

Bu yaklaşım, tüm migration geçmişini temizleyerek ve mevcut model durumuna göre tamamen yeni bir başlangıç yaparak sorunları çözdü.

## PowerShell Komut Sorunları

Windows PowerShell'de komutları birleştirmek için `&&` operatörü kullanılamaz. Bunun yerine `;` (noktalı virgül) kullanılmalıdır:

```powershell
# Hatalı komut (Windows PowerShell'de çalışmaz)
cd src/Stock.API && dotnet run

# Doğru komut (Windows PowerShell için)
cd src/Stock.API; dotnet run
```

## Teknik Detaylar

### Entity Framework Core Migration Mekanizması

Entity Framework Core, veritabanı şemasını model sınıflarına göre yönetmek için migration'ları kullanır. Migration'lar, model değişikliklerini takip eder ve bu değişiklikleri veritabanına uygular.

Migration'lar şu şekilde çalışır:
1. Model sınıflarında bir değişiklik yapılır.
2. `dotnet ef migrations add <MigrationName>` komutu ile yeni bir migration oluşturulur.
3. Migration, Up() ve Down() metotlarını içerir. Up() metodu değişiklikleri uygular, Down() metodu ise geri alır.
4. `dotnet ef database update` komutu ile migration veritabanına uygulanır.

### Migration Sorunları ve Çözüm Stratejileri

Migration sorunları genellikle şu durumlarda ortaya çıkar:
1. Aynı değişikliğin birden fazla migration'da yapılması.
2. Migration'ların yanlış sırayla uygulanması.
3. Migration'ların manuel olarak düzenlenmesi.
4. Veritabanı şemasının manuel olarak değiştirilmesi.

Çözüm stratejileri:
1. **Küçük ve Sık Migration'lar:** Büyük değişiklikler yerine, küçük ve sık migration'lar oluşturmak.
2. **Migration Temizliği:** Sorunlu migration'ları silip yeniden oluşturmak.
3. **Veritabanı Sıfırlama:** Geliştirme ortamında veritabanını tamamen silip yeniden oluşturmak.
4. **Migration Birleştirme:** Çok sayıda küçük migration'ı tek bir migration'a birleştirmek.

## Öğrenilen Dersler

1. **Büyük/Küçük Harf Duyarlılığı:** C# büyük/küçük harf duyarlı bir dildir ve property adlarının tam olarak aynı case'de kullanılması gerekir.
2. **Migration Yönetimi:** Migration sorunları biriktiğinde, bazen en iyi çözüm tüm migration'ları silip sıfırdan başlamaktır.
3. **Model Değişiklikleri:** Veritabanı şemasında bir değişiklik yapıldığında, yeni bir migration oluşturmak gerekir.
4. **PowerShell Komutları:** PowerShell'de komutları birleştirmek için `&&` yerine `;` (noktalı virgül) kullanılmalıdır.
5. **Hata Mesajlarını Okuma:** Hata mesajları genellikle sorunun kaynağını açıkça belirtir. Örneğin, "User bir UserName tanımı içermiyor" hatası, büyük/küçük harf duyarlılığı sorununa işaret eder.

## İyi Uygulamalar

1. **Tutarlı Adlandırma:** Entity property'lerini tutarlı bir şekilde adlandırmak ve bu adlandırmaya tüm kodda uymak.
2. **Düzenli Migration'lar:** Küçük ve anlamlı migration'lar oluşturmak ve her migration'a açıklayıcı bir isim vermek.
3. **Geliştirme Ortamı Temizliği:** Geliştirme ortamında veritabanını düzenli olarak temizlemek ve yeniden oluşturmak.
4. **Kod İncelemesi:** Büyük/küçük harf duyarlılığı gibi sorunları erken tespit etmek için kod incelemesi yapmak.
5. **Otomatik Testler:** Entity ve servis sınıflarını test etmek için birim testleri yazmak.

## İlgili Dosyalar

- `src/Stock.Infrastructure/Services/UserPermissionService.cs`
- `src/Stock.Domain/Entities/User.cs`
- `src/Stock.Infrastructure/Migrations/*`

## Gelecek İyileştirmeler

1. **Entity Property Adlandırma Standardı:** Entity property'leri için tutarlı bir adlandırma standardı belirlemek ve belgelemek.
2. **Migration Stratejisi:** Daha düzenli ve yönetilebilir migration'lar için bir strateji belirlemek.
3. **Otomatik Testler:** Entity ve servis sınıflarını test etmek için birim testleri eklemek.
4. **Kod Analizi:** Büyük/küçük harf duyarlılığı gibi sorunları otomatik olarak tespit etmek için kod analizi araçları kullanmak.
5. **Dokümantasyon:** Entity ve servis sınıflarını daha iyi belgelemek ve örnekler eklemek. 