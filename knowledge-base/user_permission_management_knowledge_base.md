# Kullanıcı İzinleri Yönetimi Bilgi Tabanı

## Genel Bakış
Bu belge, kullanıcı yönetimi sayfasına eklenen izin yönetimi özelliğinin teknik detaylarını ve entegrasyon sürecini açıklamaktadır. Kullanıcı izinleri yönetimi, sistem yöneticilerinin kullanıcılara özel izinler atamalarını ve bu izinleri yönetmelerini sağlayan bir özelliktir.

## Yapılan Değişiklikler

### 1. Kullanıcı Yönetimi Sayfasına İzin Yönetimi Butonu Eklenmesi
Kullanıcı yönetimi sayfasındaki her kullanıcı için izin yönetimi butonu eklendi. Bu buton, kullanıcı işlemleri bölümüne diğer butonlarla uyumlu şekilde yerleştirildi ve anahtar ikonu ile görsel olarak tanımlandı.

**Dosya:** `frontend/src/app/components/user-management/user-management.component.html`
```html
<button pButton pRipple icon="pi pi-key" class="p-button-rounded p-button-info mr-2" 
        (click)="manageUserPermissions(user)" 
        pTooltip="İzinleri Yönet" tooltipPosition="top"></button>
```

### 2. UserManagementComponent'e Yönlendirme Metodu Eklenmesi
UserManagementComponent sınıfına, kullanıcı izinleri sayfasına yönlendirme yapacak bir metot eklendi. Bu metot, kullanıcı ID'sini alarak `/users/:id/permissions` rotasına yönlendirme yapar.

**Dosya:** `frontend/src/app/components/user-management/user-management.component.ts`
```typescript
manageUserPermissions(user: any) {
  this.router.navigate([`/users/${user.id}/permissions`]);
}
```

### 3. Mevcut UserPermissionComponent ile Entegrasyon
Kullanıcı izinleri yönetimi için gerekli olan UserPermissionComponent zaten mevcuttu. Bu bileşen, kullanıcıların izinlerini görüntüleme, ekleme ve kaldırma işlevselliğini sağlar. Ayrıca, kullanıcı yönetimi sayfasına geri dönüş için bir goBack metodu içerir.

**Dosya:** `frontend/src/app/components/user-permission/user-permission.component.ts`
```typescript
goBack(): void {
  this.router.navigate(['/user-management']);
}
```

### 4. Routing Yapılandırması
Kullanıcı izinleri sayfası için gerekli routing yapılandırması zaten mevcuttu. Bu yapılandırma, `/users/:id/permissions` rotasını UserPermissionComponent'e yönlendirir ve yetkilendirme kontrolü sağlar.

**Dosya:** `frontend/src/app/app.routes.ts`
```typescript
{ 
  path: 'users/:id/permissions', 
  component: UserPermissionComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.UserManagement'
  }
}
```

## Teknik Detaylar

### Bileşen İlişkileri
- **UserManagementComponent:** Kullanıcıları listeler ve izin yönetimi butonunu içerir.
- **UserPermissionComponent:** Belirli bir kullanıcının izinlerini yönetir.
- **Router:** İki bileşen arasındaki geçişi sağlar.

### Veri Akışı
1. Kullanıcı, kullanıcı yönetimi sayfasında bir kullanıcı için izin yönetimi butonuna tıklar.
2. `manageUserPermissions` metodu çağrılır ve kullanıcı ID'si ile birlikte izin yönetimi sayfasına yönlendirme yapılır.
3. UserPermissionComponent, route parametrelerinden kullanıcı ID'sini alır ve ilgili kullanıcının izinlerini yükler.
4. Kullanıcı, izinleri yönetir ve işlem tamamlandığında "Geri Dön" butonuna tıklayarak kullanıcı yönetimi sayfasına geri döner.

### Güvenlik Kontrolleri
- İzin yönetimi sayfasına erişim, AuthGuard tarafından kontrol edilir.
- Sadece admin yetkisine sahip kullanıcılar ve 'Pages.UserManagement' iznine sahip kullanıcılar bu sayfaya erişebilir.

## Öğrenilen Dersler
1. Mevcut bileşenleri yeniden kullanmak, geliştirme sürecini hızlandırır ve kod tekrarını önler.
2. Routing yapılandırması, bileşenler arasındaki geçişleri yönetmek için önemlidir.
3. Kullanıcı arayüzünde tutarlı bir tasarım dili kullanmak, kullanıcı deneyimini iyileştirir.
4. Yetkilendirme kontrollerini route seviyesinde uygulamak, güvenliği artırır.

## Gelecek İyileştirmeler
1. İzin yönetimi arayüzü daha kullanıcı dostu hale getirilebilir.
2. İzin grupları için renk kodlaması eklenebilir.
3. İzin değişikliklerinin audit log'a kaydedilmesi sağlanabilir.
4. Toplu izin atama/kaldırma özellikleri eklenebilir.
5. İzin değişikliklerinin gerçek zamanlı olarak diğer kullanıcılara bildirilmesi sağlanabilir.

## İlgili Dosyalar
- `frontend/src/app/components/user-management/user-management.component.html`
- `frontend/src/app/components/user-management/user-management.component.ts`
- `frontend/src/app/components/user-permission/user-permission.component.html`
- `frontend/src/app/components/user-permission/user-permission.component.ts`
- `frontend/src/app/app.routes.ts` 