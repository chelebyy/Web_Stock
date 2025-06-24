# Entity Framework Core Namespace Çakışması Çözümü ve Geçiş Kılavuzu

**Son Güncelleme:** 10.07.2024

## İçindekiler

1. [Sorun Analizi](#sorun-analizi)
2. [Geçiş Stratejisi](#geçiş-stratejisi)
3. [Hazırlık Adımları](#hazırlık-adımları)
4. [Uygulama Adımları](#uygulama-adımları)
5. [Migration ve Veritabanı Güncelleme](#migration-ve-veritabanı-güncelleme)
6. [Test ve Doğrulama](#test-ve-doğrulama)
7. [Dokümantasyon ve Bilgi Paylaşımı](#dokümantasyon-ve-bilgi-paylaşımı)
8. [Öğrenilen Dersler](#öğrenilen-dersler)

## Sorun Analizi

Projemizde iki farklı namespace altında aynı isimli entity sınıfları tanımlanmıştır:

### Mevcut Durum

1. `Stock.Domain.Entities` namespace'inde:
   - `Permission` (tekil, veri yok)
   - `RolePermission` (tekil, veri yok)
   - `UserPermission` (tekil, veri yok)

2. `Stock.Domain.Entities.Permissions` namespace'inde:
   - `Permissions` (çoğul, veri var)
   - `RolePermissions` (çoğul, veri var)
   - `UserPermissions` (çoğul, veri var)

### Sorunun Etkisi

Entity Framework Core, farklı namespace'lerdeki benzer isimli sınıfları aynı tablolara eşleştirmeye çalışıyor. Bu durum şu hataya neden oluyor:

```
The entity type 'Stock.Domain.Entities.RolePermission' cannot be mapped to the table 'RolePermissions' 
because it is being mapped to 'RolePermission' by 'Stock.Domain.Entities.Permissions.RolePermission' 
and there is no relationship between the two entity types.
```

### Analiz

- Sonunda "s" olan tablolarda (`Permissions`, `RolePermissions`, `UserPermissions`) veri bulunmaktadır.
- Sonunda "s" olmayan tablolarda (`Permission`, `RolePermission`, `UserPermission`) veri bulunmamaktadır.
- Kod tabanında her iki namespace'e de referanslar vardır.

## Geçiş Stratejisi

Geçiş stratejimiz, veri içeren çoğul isimli tabloları korumak ve tekil isimli tabloları tamamen kaldırmak olacaktır. Bu yaklaşım, veri kaybı olmadan sorunun çözülmesini sağlayacaktır.

### Hedef Durum

- Sadece `Stock.Domain.Entities.Permissions` namespace'i kullanılacak
- Sadece çoğul isimli entity sınıfları (`Permissions`, `RolePermissions`, `UserPermissions`) korunacak
- Tüm referanslar bu sınıflara yönlendirilecek
- Veritabanı şeması sadece çoğul isimli tabloları içerecek

## Hazırlık Adımları

### 1. Yedekleme

- Veritabanının tam bir yedeğini al:
  ```bash
  pg_dump -h localhost -U postgres -F c -b -v -f backup.sql stockdb
  ```
- Etkilenen kod dosyalarının yedeğini al:
  ```bash
  # PowerShell
  Copy-Item -Path "src/Stock.Domain/Entities" -Destination "backup/Entities" -Recurse
  Copy-Item -Path "src/Stock.Infrastructure/Data" -Destination "backup/Data" -Recurse
  ```

### 2. Etkilenen Dosyaların Belirlenmesi

- **Entity Sınıfları**:
  - `Stock.Domain.Entities.Permission.cs`
  - `Stock.Domain.Entities.RolePermission.cs`
  - `Stock.Domain.Entities.UserPermission.cs`
  - `Stock.Domain.Entities.Permissions.Permission.cs`
  - `Stock.Domain.Entities.Permissions.RolePermission.cs`
  - `Stock.Domain.Entities.Permissions.UserPermission.cs`

- **DbContext Sınıfı**: 
  - `ApplicationDbContext.cs`

- **Repository ve Servis Sınıfları**: 
  - `PermissionRepository.cs`
  - `PermissionService.cs`
  - `UserPermissionService.cs`
  - İlgili diğer servis ve repository sınıfları

### 3. Değişiklik Yaklaşımı

- Önce tekil isimli sınıfları kaldır
- Ardından tüm referansları çoğul isimli sınıflara yönlendir
- Son olarak yeni bir migration oluştur ve uygula

## Uygulama Adımları

### Adım 1: Tekil İsimli Entity Sınıflarının Kaldırılması

- `Stock.Domain.Entities.Permission.cs`, `Stock.Domain.Entities.RolePermission.cs` ve `Stock.Domain.Entities.UserPermission.cs` dosyalarını kaldır veya yeniden adlandır
- Bu sınıflara olan doğrudan referansları belirle

### Adım 2: DbContext Sınıfının Güncellenmesi

- `ApplicationDbContext.cs` içindeki DbSet tanımlarını güncelle:

```csharp
// Eski kod
public DbSet<Permission> Permissions { get; set; }
public DbSet<RolePermission> RolePermissions { get; set; }
public DbSet<UserPermission> UserPermissions { get; set; }

// Yeni kod
public DbSet<Permissions.Permissions> Permissions { get; set; }
public DbSet<Permissions.RolePermissions> RolePermissions { get; set; }
public DbSet<Permissions.UserPermissions> UserPermissions { get; set; }
```

- OnModelCreating metodundaki entity konfigürasyonlarını güncelle

### Adım 3: Repository Sınıflarının Güncellenmesi

- İlgili repository sınıflarındaki entity tiplerini güncelle
- LINQ sorgularında kullanılan navigation property'leri güncelle

### Adım 4: Servis Sınıflarının Güncellenmesi

- İlgili servis sınıflarındaki entity tiplerini güncelle
- Metot parametrelerinde ve dönüş değerlerinde kullanılan entity ve DTO tiplerini güncelle
- İş mantığında tekil isimli entity sınıflarına yapılan referansları değiştir

### Adım 5: Diğer Bağımlı Kodların Güncellenmesi

- Controller sınıflarını güncelle
- View modellerini ve DTO'ları güncelle
- Seed verileri ve migrationları kontrol et
- Extension metodlarını güncelle

### Adım 6: Uygun Using Direktiflerinin Eklenmesi

- Tüm dosyalarda `using Stock.Domain.Entities;` yerine `using Stock.Domain.Entities.Permissions;` kullan
- Gerektiğinde alias tanımla: `using Permissions = Stock.Domain.Entities.Permissions.Permissions;`

## Migration ve Veritabanı Güncelleme

### 1. Migration Oluşturma

```bash
dotnet ef migrations add ConsolidatePermissionEntities --project src/Stock.Infrastructure --startup-project src/Stock.API
```

### 2. Migration Kodunun İncelenmesi

- Oluşturulan migration dosyasını inceleyerek, beklenmeyen değişikliklerin olup olmadığını kontrol et
- Özellikle tablo isimlendirme ve indeksleri kontrol et
- Veri kaybına neden olabilecek değişiklikleri belirle

### 3. Veritabanını Güncelleme

```bash
dotnet ef database update --project src/Stock.Infrastructure --startup-project src/Stock.API
```

### 4. Sorun Giderme

- Migration uygulama sırasında oluşabilecek hataları belirle
- Hata durumları için geri dönüş stratejileri:
  - Veritabanını yedekten geri yükleme:
    ```bash
    pg_restore -h localhost -U postgres -d stockdb -c backup.sql
    ```
  - Eski kodu geri getirme
  - Manuel SQL betikleri çalıştırma

## Test ve Doğrulama

### 1. Uygulama Testi

- Uygulamayı başlat:
  ```bash
  dotnet run --project src/Stock.API
  ```
- Temel fonksiyonları test et
- Özellikle izin kontrolü, rol atama ve kullanıcı izinleri işlevlerini test et
- Performans sorunlarını veya beklenmeyen davranışları kontrol et

### 2. Veritabanı Doğrulama

- Veritabanı şemasını kontrol et: tablolar, ilişkiler ve indeksler
- Veri bütünlüğünü doğrula: özellikle ilişkili verilerin doğru şekilde bağlantılı olduğundan emin ol
- Sorguların doğru çalıştığını doğrula

### 3. Hata Ayıklama

- Uygulama loglarını kontrol et
- EF Core sorgu loglarını inceleyerek performans sorunlarını belirle
- Potansiyel N+1 sorgu sorunlarını tespit et

### 4. Güvenlik Doğrulama

- İzin kontrollerinin doğru çalıştığını doğrula
- Yetkisiz erişimlerin engellendiğini kontrol et

## Dokümantasyon ve Bilgi Paylaşımı

### 1. Değişiklik Dökümantasyonu

- Yapılan tüm değişiklikleri belgelendir
- Entity sınıfları üzerindeki değişiklikleri listele
- Migration detaylarını not et
- Karşılaşılan sorunları ve çözümleri belgelendir

### 2. Takıma Bilgi Verme

- Yapılan değişiklikleri takımla paylaş
- Yeni kod yapısını açıkla
- Geliştiricilerin dikkat etmesi gereken noktaları vurgula

## Öğrenilen Dersler

### 1. Entity Framework Core'un Davranışı

- EF Core, farklı namespace'lerdeki aynı isimli entity sınıflarını, ilgili bir bağlantı olmadığı sürece aynı tabloya eşleştirmeye çalıştığında sorun çıkarır.
- Entity adlandırma standartları (tekil/çoğul) tutarlı olmalıdır.

### 2. Kod Organizasyonu

- Namespace organizasyonu, entity sınıfları için tutarlı olmalıdır.
- Bir entity sınıfı, projenin her yerinde aynı namespace'den referans edilmelidir.

### 3. Veritabanı Şema Yönetimi

- Veritabanı şeması değişiklikleri dikkatlice planlanmalıdır.
- Migration öncesi her zaman yedek alınmalıdır.
- Üretim ortamına geçmeden önce test ortamında değişiklikler doğrulanmalıdır.

### 4. İleriye Dönük Önlemler

- Entity adlandırma standartları belirle (tekil veya çoğul isimlendirme tutarlı olmalı)
- Namespace organizasyonu için kurallar oluştur
- Code review süreçlerini güçlendir
- Migration oluşturmadan önce kod tabanında çakışma kontrolü yap 