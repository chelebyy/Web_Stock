# Rol Sayfası Erişim İzinleri Bilgi Tabanı

## Genel Bakış

Bu bilgi tabanı, rol yönetimi sayfasındaki sayfa erişim izinleri için yapılan geliştirmeleri içermektedir. Bu geliştirmeler, kullanıcıların hangi sayfalara erişebileceğini rol bazlı olarak yönetmeyi sağlar.

## Yapılan İşlemler

### Backend Tarafı
1. Yeni "Sayfa Erişimi" izin grubu oluşturuldu
2. Sayfalar için özel izinler tanımlandı (Pages.AdminDashboard, Pages.UserDashboard, vb.)
3. Seed verilerinde bu izinlerin eklenmesi yapıldı

### Frontend Tarafı
1. RoleDetailComponent sayfası görsel olarak yenilendi
2. İzinler genişletilip daraltılabilir panellere dönüştürüldü
3. Sayfa erişim izinleri için toggle butonlar eklendi
4. AuthGuard'a sayfa erişim kontrolü eklendi
5. Rotalara gerekli erişim izinleri tanımlandı

## Tasarım ve Kullanıcı Deneyimi

### Sayfa Erişim İzinleri Bölümü
- Sayfa erişim izinleri için özel bir bölüm oluşturuldu
- Toggle butonlar ile kolay açma/kapama
- Yeşil/gri renklerle aktif/pasif durumlar gösterildi
- Kilit ikonları ile erişim durumu görselleştirildi
- Erişim kartları için hover efektleri eklendi

### Diğer İzinler Bölümü
- İzinler gruplarına göre akordeon panellere yerleştirildi
- Gruplar daraltılıp genişletilebilir
- Her grubun kendi başlığı var
- İzinler için açıklamalar eklendi

## API ve Veri Modeli

### Yeni İzinler
```csharp
// Sayfa Erişim İzinleri
new Permission { Name = "Pages.AdminDashboard", Description = "Admin paneline erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.UserDashboard", Description = "Kullanıcı paneline erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.UserManagement", Description = "Kullanıcı yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.RoleManagement", Description = "Rol yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.StockManagement", Description = "Stok yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.Reports", Description = "Raporlar sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
new Permission { Name = "Pages.Settings", Description = "Ayarlar sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow }
```

### Rotalara İzin Tanımları
```typescript
{
  path: 'admin-dashboard', 
  component: AdminDashboardComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.AdminDashboard'
  }
}
```

## Karşılaşılan Hatalar ve Çözümleri

### Hata 1: PrimeNG Bileşenleri Görüntülenemedi
**Çözüm:** Bileşen dosyasında gerekli PrimeNG modülleri import edildi ve imports dizisine eklendi:
```typescript
@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AccordionModule,
    ToggleButtonModule,
    CheckboxModule,
    ButtonModule,
    ProgressSpinnerModule
  ]
})
```

### Hata 2: İzin Gruplarına Erişilemedi
**Çözüm:** Permission servisinden izinleri grup bazlı döndürmek için yardımcı metotlar eklendi:
```typescript
getPermissionsByGroup(groupName: string): Permission[] {
  const group = this.permissionGroups.find(g => g.group === groupName);
  return group ? group.permissions : [];
}
```

## Kullanım Örnekleri

### Sayfa Erişim İzinlerinin Kontrolü
```typescript
// AuthGuard içinde
const pagePermission = this.pagePermissionMap[url];
if (pagePermission) {
  if (!this.authService.hasPermission(pagePermission)) {
    console.log(`Erişim reddedildi: '${pagePermission}' izni yok`);
    this.router.navigate(['/user-dashboard']);
    return false;
  }
}
```

### HTML'de Sayfa Erişim İzinlerinin Gösterilmesi
```html
<div *ngFor="let permission of getPermissionsByGroup('Sayfa Erişimi')">
  <p-toggleButton 
    [onLabel]="permission.name.split('.')[1]" 
    [offLabel]="permission.name.split('.')[1]"
    [onIcon]="'pi pi-lock-open'" 
    [offIcon]="'pi pi-lock'"
    [styleClass]="isPermissionSelected(permission.id) ? 'access-enabled' : 'access-disabled'"
    [checked]="isPermissionSelected(permission.id)"
    (onChange)="togglePermission(permission.id)">
  </p-toggleButton>
</div>
```

## Öğrenilen Dersler

- İzinleri mantıksal gruplar halinde düzenlemek kullanıcı deneyimini artırıyor
- Toggle butonlar sayfa erişim kontrolü için daha sezgisel bir arayüz sağlıyor
- Standalone bileşenler kullanılırken gerekli tüm modüllerin imports dizisine eklenmesi gerekiyor
- İzin kontrollerini tek bir yerde (AuthGuard) toplamak kod tekrarını önlüyor
- AuthGuard'da hem route data özelliğinden hem de URL tabanlı izin kontrolü yapılabilir

## Gelecek Geliştirmeler

- İzin değişikliklerinin gerçek zamanlı etki etmesi için login sonrası token yenileme
- Grup bazlı toplu izin atama/kaldırma özelliği
- İzin geçmişi loglaması ve değişikliklerin izlenmesi
- Sayfa erişim izinleri için detaylı istatistikler ve raporlama
- Özel sayfa grupları oluşturma ve bunlara toplu izin atama 