# İzin Yönetimi Namespace Çakışması Çözümü

## Sorun Tanımı

Stock.Domain katmanında aynı isimde ancak farklı namespace'lerde (Stock.Domain.Entities ve Stock.Domain.Entities.Permissions) bulunan entity sınıfları derleme ve veritabanı migrasyon hatalarına neden oluyordu:

- `Permission`
- `UserPermission`
- `RolePermission`

Bu durum, aşağıdaki hataya neden oluyordu:

```
error CS0104: 'Permission', 'Stock.Domain.Entities.Permission' ile 'Stock.Domain.Entities.Permissions.Permission' arasında belirsiz bir başvuru
```

Ayrıca, Entity Framework Core konfigürasyonlarında yanlış namespace'deki entity'ler referans edildiği için, model oluşturma sırasında da hatalar meydana geliyordu.

## Tespit Edilen Sorunlar

1. Domain katmanında duplicate entity sınıfları
2. Konfigürasyon dosyalarında yanlış namespace kullanımı
3. Repository ve servis sınıflarında farklı namespace'lerden entity'lerin kullanılması
4. Migrasyon eksikliği

## Çözüm Adımları

### 1. Namespace Tutarlılığı

Entity Framework Core konfigürasyon dosyalarını düzelttik:

```csharp
// Önce
using Stock.Domain.Entities.Permissions;

// Sonra
using Stock.Domain.Entities;
```

Bu düzeltmeyi aşağıdaki dosyalarda yaptık:
- `src/Stock.Infrastructure/Data/Config/UserPermissionConfiguration.cs`
- `src/Stock.Infrastructure/Data/Config/RolePermissionConfiguration.cs`

### 2. Çakışan Dosyaların Temizlenmesi

Duplicate olan konfigürasyon dosyalarını sildik:
- `src/Stock.Infrastructure/Data/Configurations/UserPermissionConfiguration.cs`
- `src/Stock.Infrastructure/Data/Configurations/RolePermissionConfiguration.cs`

### 3. Servislerin Güncellenmesi

Servis ve repository sınıflarını güncelleyerek tutarlı namespace kullanımını sağladık:

```csharp
// Stock.Infrastructure/Services/UserPermissionService.cs
var userPermission = new Stock.Domain.Entities.UserPermission
{
    UserId = userId,
    PermissionId = permissionId,
    IsGranted = isGranted,
    CreatedAt = DateTime.UtcNow
};
```

### 4. Migrasyon Oluşturma ve Uygulama

Entity Framework Core modelini güncellemek için yeni bir migrasyon oluşturduk:

```powershell
dotnet ef migrations add FixPermissionEntityNamespaces --project Stock.Infrastructure --startup-project Stock.API
```

Ve bu migrasyonu veritabanına uyguladık:

```powershell
dotnet ef database update --project Stock.Infrastructure --startup-project Stock.API
```

## Çözüm Sonrası

- Backend derleme hataları ortadan kalktı
- Veritabanı migrasyon sorunları çözüldü
- Uygulama başarıyla çalışıyor

## Öğrenilen Dersler

1. Domain katmanında entity'leri tasarlarken, tutarlı bir namespace stratejisi belirlenmelidir
2. Aynı isimde entity'ler farklı namespace'lerde olmamalıdır
3. EF Core konfigürasyonlarında doğru entity tiplerinin referans edildiğinden emin olunmalıdır
4. Model değişikliklerinden sonra hemen migrasyon oluşturulmalı ve uygulanmalıdır
5. Veri tutarlılığını korumak için mevcut entity verileri düşünülerek migrasyon stratejisi belirlenmelidir

## Sonraki Adımlar

1. Uzun vadede, çakışan entity'lerin tam olarak birleştirilmesi veya yeniden yapılandırılması gerekebilir
2. Gelecekteki refactor işlemlerinde domain model incelenmelidir
3. Mevcut migrasyonların temizlenmesi ve birleştirilmesi düşünülebilir
4. Entity ilişkilerinin ve mimarinin gözden geçirilmesi yararlı olabilir 