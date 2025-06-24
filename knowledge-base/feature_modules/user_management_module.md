# User Management Feature Modülü

## Genel Bakış

User Management Feature Modülü, sistemdeki kullanıcıların oluşturulması, düzenlenmesi, silinmesi ve yönetilmesi işlevlerini sağlayan bir Angular 19 feature modülüdür. Bu modül, Angular 19 migrasyonu kapsamında feature-based ve lazy-loading yapısına uygun olarak yeniden düzenlenmiştir.

## Modül Yapısı

```
frontend/src/app/features/user-management/
├── components/
│   ├── user-management.component.ts
│   ├── user-management.component.html
│   ├── user-management.component.scss
│   ├── user-permission/
│   │   ├── user-permission.component.ts
│   │   ├── user-permission.component.html
│   │   └── user-permission.component.scss
├── services/
│   ├── user.service.ts
│   └── user-permission.service.ts
└── user-management.routes.ts
```

## Bileşenler

### UserManagementComponent

**Amaç**: Tüm kullanıcıları listeler, yeni kullanıcı oluşturma, kullanıcı düzenleme ve silme işlemlerini sağlar.

**Özellikler**:
- Kullanıcı listeleme
- Kullanıcı ekleme/düzenleme form diyaloğu
- Şifre değiştirme
- Kullanıcı silme onay diyaloğu
- Kullanıcı arama/filtreleme
- Kullanıcı rollerini atama

**Route**: 
- `/user-management`
- `/users` (alternatif)

### UserPermissionComponent

**Amaç**: Belirli bir kullanıcının özel izinlerini düzenleme imkanı sağlar.

**Özellikler**:
- Kullanıcıya özel izin atama
- Rol bazlı izinleri görüntüleme
- Özel izin ekleme/kaldırma
- İzin değişikliğini kaydetme

**Route**: `/users/:userId/permissions`

## Servisler

### UserService

**Amaç**: Kullanıcılarla ilgili API isteklerini yönetir.

**Metotlar**:
- `getUsers()`: Tüm kullanıcıları getirir
- `getUser(id)`: Belirli bir kullanıcıyı getirir
- `createUser(user)`: Yeni kullanıcı oluşturur
- `updateUser(user)`: Kullanıcı bilgilerini günceller
- `deleteUser(id)`: Kullanıcı siler
- `changePassword(userId, passwords)`: Kullanıcı şifresini değiştirir
- `getUsersByRole(roleId)`: Belirli bir role sahip kullanıcıları getirir

### UserPermissionService

**Amaç**: Kullanıcı izinleriyle ilgili API isteklerini yönetir.

**Metotlar**:
- `getUserPermissions(userId)`: Kullanıcı izinlerini getirir
- `updateUserPermissions(userId, permissions)`: Kullanıcı izinlerini günceller
- `getUserCustomPermissions(userId)`: Kullanıcıya özel atanmış izinleri getirir

## API Entegrasyonu

Modül aşağıdaki API endpoint'leri ile entegre çalışır:

- `GET /api/users`: Tüm kullanıcıları listeler
- `GET /api/users/{id}`: Belirli bir kullanıcının detaylarını getirir
- `POST /api/users`: Yeni kullanıcı oluşturur
- `PUT /api/users/{id}`: Kullanıcı bilgilerini günceller
- `DELETE /api/users/{id}`: Kullanıcı siler
- `POST /api/users/{id}/change-password`: Kullanıcı şifresini değiştirir
- `GET /api/permissions/user/{userId}`: Kullanıcının izinlerini getirir
- `PUT /api/permissions/user/{userId}`: Kullanıcının izinlerini günceller
- `GET /api/permissions/user/{userId}/custom`: Kullanıcıya özel izinleri getirir

## Lazy Loading

Modül, lazy loading prensibine göre yapılandırılmıştır ve app.routes.ts içinde şu şekilde tanımlanmıştır:

```typescript
{
  path: '',
  loadChildren: () => import('./features/user-management/user-management.routes').then(m => m.USER_MANAGEMENT_ROUTES)
}
```

user-management.routes.ts dosyası şu rotaları tanımlar:

```typescript
export const USER_MANAGEMENT_ROUTES: Routes = [
  {
    path: 'user-management',
    component: UserManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.UserManagement' }
  },
  {
    path: 'users',
    component: UserManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.UserManagement' }
  },
  {
    path: 'users/:userId/permissions',
    component: UserPermissionComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.UserManagement' }
  }
];
```

## Migrasyon Notları

Bu modül, Angular 19 migrasyonu kapsamında aşağıdaki değişikliklere uğramıştır:

1. Components ve services dizinleri oluşturularak ilgili dosyalar taşındı
2. Standalone component yaklaşımına geçiş yapıldı
3. user-management.routes.ts dosyası oluşturularak lazy loading desteklendi
4. Import yolları güncellendi
5. UI iyileştirmeleri yapıldı (PrimeNG 19 uyumluluğu sağlandı)
6. Sicil alanı güncellemeleri ve benzersizlik kontrolleri eklendi

## Bilinen Sorunlar ve Çözümleri

1. **Kullanıcı Güncelleme ID Sorunu**: URL'deki ID ile request body içindeki ID eşleşmezse 400 Bad Request hatası alınıyor. Bu sorunu çözmek için updateUser fonksiyonu güncellendi.

2. **Sicil Numarası Benzersizlik Kontrolü**: Sicil numarası alanı için benzersizlik kontrolü eklendi. Aynı sicil numarasına sahip kullanıcı oluşturulması engellendi.

3. **Şifre Değiştirme Sorunları**: Şifre değiştirme formunda CORS sorunları yaşandı, API'nin hem /api/users/{id}/change-password hem de /api/account/change-password endpointleri düzgün çalışacak şekilde güncellendi.

## Test Senaryoları

- Kullanıcı listeleme
- Kullanıcı oluşturma (farklı rollerle)
- Kullanıcı güncelleme
- Kullanıcı silme
- Şifre değiştirme
- Özel izin atama
- Lazy loading çalışma durumu
- Yetkisiz erişim durumu
- Sicil numarası benzersizlik kontrolü

## Bağlantılı Belgeler

- [Angular 19 Migration](../angular-19-migration/angular19_migration.md)
- [Permission Management](../user_permission_management_knowledge_base.md)
- [Sicil Alanı Ekleme](../sicil_alani_ekleme.md)
- [Kullanıcı Silme Sorunu](../kullanici_silme_sorunu.md)
- [Errors & Solutions](../../errors.md) 