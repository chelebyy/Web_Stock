# Role Management Feature Modülü

## Genel Bakış

Role Management Feature Modülü, sistemdeki kullanıcı rollerini tanımlama, düzenleme ve yönetme işlevlerini sağlayan bir Angular 19 feature modülüdür. Bu modül, Angular 19 migrasyonu kapsamında feature-based ve lazy-loading yapısına uygun olarak yeniden düzenlenmiştir.

## Modül Yapısı

```
frontend/src/app/features/role-management/
├── components/
│   ├── role-management.component.ts
│   ├── role-management.component.html
│   ├── role-management.component.scss
│   ├── role-detail/
│   │   ├── role-detail.component.ts
│   │   ├── role-detail.component.html
│   │   └── role-detail.component.scss
├── services/
│   └── role.service.ts
└── role-management.routes.ts
```

## Bileşenler

### RoleManagementComponent

**Amaç**: Tüm rolleri listeler, yeni rol oluşturma, rol düzenleme ve silme işlemlerini sağlar.

**Özellikler**:
- Rol listeleme
- Rol ekleme/düzenleme form diyaloğu
- Rol silme onay diyaloğu
- Rol arama/filtreleme

**Route**: 
- `/role-management`
- `/roles` (alternatif)

### RoleDetailComponent

**Amaç**: Belirli bir rolün detaylarını ve izinlerini düzenleme imkanı sağlar.

**Özellikler**:
- Rol bilgileri düzenleme
- Rol izinlerini ayarlama (grup bazlı izin atama)
- İzin gruplarını görselleştirme
- İzin değişikliğini kaydetme

**Route**: `/roles/:roleId`

## Servisler

### RoleService

**Amaç**: Rol ile ilgili API isteklerini yönetir.

**Metotlar**:
- `getRoles()`: Tüm rolleri getirir
- `getRole(id)`: Belirli bir rolü getirir
- `createRole(role)`: Yeni rol oluşturur
- `updateRole(role)`: Rol bilgilerini günceller
- `deleteRole(id)`: Rol siler
- `getRolePermissions(roleId)`: Rolün izinlerini getirir
- `updateRolePermissions(roleId, permissions)`: Rolün izinlerini günceller

## API Entegrasyonu

Modül aşağıdaki API endpoint'leri ile entegre çalışır:

- `GET /api/roles`: Tüm rolleri listeler
- `GET /api/roles/{id}`: Belirli bir rolün detaylarını getirir
- `POST /api/roles`: Yeni rol oluşturur
- `PUT /api/roles/{id}`: Rol bilgilerini günceller
- `DELETE /api/roles/{id}`: Rol siler
- `GET /api/permissions/role/{roleId}`: Rolün izinlerini getirir
- `PUT /api/permissions/role/{roleId}`: Rolün izinlerini günceller

## Lazy Loading

Modül, lazy loading prensibine göre yapılandırılmıştır ve app.routes.ts içinde şu şekilde tanımlanmıştır:

```typescript
{
  path: '',
  loadChildren: () => import('./features/role-management/role-management.routes').then(m => m.ROLE_MANAGEMENT_ROUTES)
}
```

role-management.routes.ts dosyası şu rotaları tanımlar:

```typescript
export const ROLE_MANAGEMENT_ROUTES: Routes = [
  {
    path: 'role-management',
    component: RoleManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.RoleManagement' }
  },
  {
    path: 'roles',
    component: RoleManagementComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.RoleManagement' }
  },
  {
    path: 'roles/:roleId',
    component: RoleDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.RoleManagement' }
  }
];
```

## Migrasyon Notları

Bu modül, Angular 19 migrasyonu kapsamında aşağıdaki değişikliklere uğramıştır:

1. Components ve services dizinleri oluşturularak ilgili dosyalar taşındı
2. Standalone component yaklaşımına geçiş yapıldı
3. role-management.routes.ts dosyası oluşturularak lazy loading desteklendi
4. Import yolları güncellendi
5. UI iyileştirmeleri yapıldı (PrimeNG 19 uyumluluğu sağlandı)

## Test Senaryoları

- Rol listeleme
- Rol oluşturma
- Rol güncelleme
- Rol silme
- Rol izinlerini değiştirme
- Lazy loading çalışma durumu
- Yetkisiz erişim durumu

## Bağlantılı Belgeler

- [Angular 19 Migration](../angular-19-migration/angular19_migration.md)
- [Permission Management](../user_permission_management_knowledge_base.md)
- [Errors & Solutions](../../errors.md) 