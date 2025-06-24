# Kullanılmayan Sayfa İzinleri Temizleme

## Genel Bakış

Bu belge, henüz oluşturulmamış sayfalar için rol yönetimi ekranında görüntülenen izinlerin temizlenmesi sürecini açıklar. Kullanıcı deneyimini iyileştirmek için, mevcut olmayan sayfalara ait izinlerin kaldırılması gerekmektedir.

## Sorun

Rol yönetimi ekranında, henüz oluşturulmamış sayfalar için izinler görüntülenmekteydi:

- Reports (Raporlar) sayfası
- Settings (Ayarlar) sayfası
- StockManagement (Stok Yönetimi) sayfası
- Stok Yönetimi grubundaki tüm izinler
- Revir sayfası
- BilgiIslem sayfası
- Egitim sayfası

Bu izinlerin gösterilmesi, kullanıcılarda kafa karışıklığına neden olabilir ve mevcut olmayan işlevler beklentisi oluşturabilir.

## Çözüm

Kullanılmayan sayfa izinlerini temizlemek için üç yaklaşım uygulandı:

### 1. Backend Çözümü: AdminController'da Temizleme Endpoint'i

Veritabanından kullanılmayan izinleri kalıcı olarak silmek için `AdminController` sınıfında bir temizleme endpoint'i oluşturuldu:

```csharp
[HttpDelete("cleanup-unused-permissions")]
public async Task<IActionResult> CleanupUnusedPermissions()
{
    // Kullanılmayan izinleri temizleme kodu
    // ...
}
```

Bu endpoint aşağıdaki izinleri temizler:
- Pages.Reports
- Pages.Settings
- Pages.StockManagement
- Stok Yönetimi grubundaki tüm izinler
- Pages.Revir.*
- Pages.BilgiIslem.*
- Pages.Egitim.*

İzinler temizlenirken, ilişkili `UserPermission` ve `RolePermission` kayıtları da silinir.

### 2. Frontend Çözümü: RoleDetailComponent'te Filtreleme

Frontend tarafında, `RoleDetailComponent` sınıfındaki izin yönetimi iki şekilde güncellendi:

1. `getPagePermissionsByCategory` metodu güncellenerek, kullanılmayan sayfaların izinleri filtrelendi:

```typescript
getPagePermissionsByCategory(category: string): Permission[] {
  const pagePermissions = this.getPermissionsByGroup('Sayfa Erişimi');
  
  // Platformda olmayan sayfaların izinlerini filtrele
  const excludedPermissions = [
    'Pages.Reports', 
    'Pages.Settings', 
    'Pages.StockManagement',
    'Pages.Revir.View', 'Pages.Revir.Create', 'Pages.Revir.Edit', 'Pages.Revir.Delete',
    'Pages.BilgiIslem.View', 'Pages.BilgiIslem.Create', 'Pages.BilgiIslem.Edit', 'Pages.BilgiIslem.Delete',
    'Pages.Egitim.View', 'Pages.Egitim.Create', 'Pages.Egitim.Edit', 'Pages.Egitim.Delete'
  ];
  
  // Mevcut izinleri filtrele
  const filteredPermissions = pagePermissions.filter(p => !excludedPermissions.includes(p.name));
  
  // Kategoriye göre filtreleme devam eder...
}
```

2. `getGroupsExcept` metodu güncellenerek, Diğer İzinler bölümünde "Stok Yönetimi" kategorisinin gösterilmemesi sağlandı:

```typescript
getGroupsExcept(excludeGroupName: string): string[] {
  // Filtrelenecek grup isimleri
  const excludedGroups = [excludeGroupName, 'Stok Yönetimi'];
  
  // Tüm gruplardan filtrelenecek grupları çıkar
  const groupNames = this.permissionGroups
    .map(g => g.group)
    .filter(g => g && !excludedGroups.includes(g));
    
  return [...new Set(groupNames)]; // Tekrar eden grup isimlerini temizle
}
```

### 3. SQL Script Oluşturulması

Veritabanı yöneticileri için, elle uygulanabilecek bir SQL script de oluşturuldu:

```sql
-- Kullanılmayan sayfa izinlerini silme scripti

-- Reports sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Reports');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.Reports');
DELETE FROM "Permissions" WHERE "Name" = 'Pages.Reports';

-- StockManagement sayfası izinleri
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.StockManagement');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Name" = 'Pages.StockManagement');
DELETE FROM "Permissions" WHERE "Name" = 'Pages.StockManagement';

-- Stok Yönetimi grubundaki tüm izinler
DELETE FROM "UserPermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Group" = 'Stok Yönetimi');
DELETE FROM "RolePermissions" WHERE "PermissionId" IN (SELECT "Id" FROM "Permissions" WHERE "Group" = 'Stok Yönetimi');
DELETE FROM "Permissions" WHERE "Group" = 'Stok Yönetimi';

-- ... diğer izinler için benzer sorgular
```

## Sonuç

Bu değişiklikler sayesinde:

1. Kullanıcılar artık sadece mevcut sayfalar için izinleri görecek
2. "Stok Yönetimi" başlığı altındaki izinler, Diğer İzinler bölümünde gösterilmeyecek
3. Rol yönetimi ekranı daha temiz ve anlaşılır hale geldi
4. Veritabanı ve uygulama gereksiz verilerden arındırıldı

## İlgili Dosyalar

- `src/Stock.API/Controllers/AdminController.cs`
- `frontend/src/app/components/role-management/role-detail/role-detail.component.ts`
- `scripts/remove_unused_permissions.sql` 