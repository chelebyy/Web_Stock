# İzin Yönetimi Bilgi Tabanı

## Genel Bakış
Bu bilgi tabanı, izin tabanlı erişim kontrolü (PBAC) sisteminin uygulanması sırasında yapılan işlemleri, karşılaşılan hataları ve çözüm yöntemlerini içerir.

## Yapılan İşlemler

### Backend Tarafı
1. Permission ve RolePermission entity sınıfları oluşturuldu
2. IPermissionService ve PermissionService sınıfları oluşturuldu
3. PermissionsController API controller'ı oluşturuldu
4. RequirePermission özniteliği oluşturuldu

### Frontend Tarafı
1. Permission model sınıfı oluşturuldu
2. PermissionService servisi oluşturuldu
3. AuthService sınıfına hasPermission metodu eklendi
4. HasPermissionDirective direktifi oluşturuldu
5. AuthGuard sınıfı izin kontrolü için güncellendi
6. RoleDetailComponent bileşeni oluşturuldu

## Karşılaşılan Hatalar ve Çözümleri

### Hata 1: RoleService'de getRoleById metodu eksikti
**Çözüm:** RoleService sınıfına getRoleById metodu eklendi.

### Hata 2: App routing modülünde Routes ve AuthGuard tanımlı değildi
**Çözüm:** Gerekli import ifadeleri eklendi:
```typescript
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
```

## Kullanım Örnekleri

### HTML Şablonunda İzin Kontrolü
```html
<button *hasPermission="'Users.Create'" pButton label="Yeni Kullanıcı Ekle"></button>
```

### Rota Tanımında İzin Kontrolü
```typescript
{
  path: 'users',
  component: UserListComponent,
  canActivate: [AuthGuard],
  data: { requiredPermission: 'Users.View' }
}
```

### Servis Metodunda İzin Kontrolü
```typescript
if (this.authService.hasPermission('Users.Delete')) {
  // Silme işlemi gerçekleştir
}
```

## Notlar
- Admin rolündeki kullanıcılar otomatik olarak tüm izinlere sahiptir
- İzinler JWT token içinde taşınır ve client tarafında doğrulanır
- Güvenlik açısından kritik işlemler için sunucu tarafında da izin kontrolü yapılmalıdır 