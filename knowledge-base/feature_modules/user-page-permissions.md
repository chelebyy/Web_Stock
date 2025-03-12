# Kullanıcı İzinleri Sayfası (User Page Permissions)

## Genel Bakış

Kullanıcı İzinleri sayfası, dashboard sayfalarına erişim izinlerini yönetmek için kullanılan bir arayüzdür. Bu sayfa, hangi kullanıcıların hangi dashboard sayfalarına erişebileceğini kontrol etmeyi sağlar.

## Sayfa Yapısı

Sayfa iki ana sekmeden oluşur:
1. **Kullanıcılar**: Tüm kullanıcıları listeler ve izin durumlarını gösterir
2. **İzinler**: Belirli bir sayfaya erişim izni olan kullanıcıları listeler

## Bileşenler

### UserPagePermissionsComponent

Bu bileşen, kullanıcı izinlerini yönetmek için ana bileşendir. Kullanıcıları listeler, izin verme ve kaldırma işlemlerini gerçekleştirir.

### DashboardPermissionService

Bu servis, izin yönetimi için gerekli API çağrılarını yapar:
- Kullanıcıları getirme
- Sayfa izinlerini getirme
- İzin verme
- İzin kaldırma

## Karşılaşılan Sorunlar ve Çözümleri

### 1. Durum Etiketlerinin Görünümü

**Sorun**: Durum sütunundaki "Aktif" ve "Pasif" etiketleri doğru metni gösteriyordu ancak renkler ve ikonlar görünmüyordu.

**Çözüm**: 
1. CSS dosyasında `.p-tag` sınıfı için özel stiller tanımlandı
2. `custom-tag` adında özel bir sınıf eklendi
3. Başarı (yeşil) ve tehlike (kırmızı) durumları için özel stiller tanımlandı
4. İkonlar için `content` özelliği kullanılarak görsel göstergeler eklendi
5. HTML'de inline stil kullanılarak renklerin kesin olarak görünmesi sağlandı

```css
.custom-tag.p-tag.p-tag-success {
  background-color: var(--green-500, #22c55e) !important;
  border: 1px solid var(--green-600, #16a34a) !important;
  color: white !important;
}

.custom-tag.p-tag.p-tag-success:before {
  content: "\e930" !important; /* pi pi-check-circle icon */
  font-family: 'primeicons' !important;
  margin-right: 0.25rem !important;
}
```

```html
<p-tag [value]="user.hasPagePermission ? 'Aktif' : 'Pasif'" 
       [severity]="user.hasPagePermission ? 'success' : 'danger'"
       styleClass="custom-tag"
       [style]="user.hasPagePermission ? 
                {'background-color': '#22c55e', 'color': 'white', 'border': '1px solid #16a34a'} : 
                {'background-color': '#ef4444', 'color': 'white', 'border': '1px solid #dc2626'}"></p-tag>
```

### 2. "İzin Ver" Butonu Görünümü

**Sorun**: "İzin Ver" butonu yeterince belirgin değildi ve kullanıcı deneyimini iyileştirmek gerekiyordu.

**Çözüm**:
1. Butonun stilini `p-button-primary` yerine `p-button-outlined p-button-success` olarak değiştirdik
2. Butonun minimum genişliğini 120px olarak ayarladık
3. `white-space: nowrap` ekleyerek metnin taşmasını önledik

```css
.p-button.p-button-success {
  background-color: #22c55e;
  border-color: #22c55e;
  min-width: 120px;
  white-space: nowrap;
}
```

## Öğrenilen Dersler

1. **PrimeNG Bileşenlerinin Özelleştirilmesi**: PrimeNG bileşenlerini özelleştirirken, bazen CSS değişkenlerini kullanmak yeterli olmayabilir. Bu durumda, doğrudan stil tanımlamaları ve `!important` kullanımı gerekebilir.

2. **Çoklu Yaklaşım**: Stil sorunlarını çözerken, hem CSS sınıfları hem de inline stiller kullanarak daha güvenilir sonuçlar elde edilebilir.

3. **İkon Kullanımı**: PrimeNG'nin ikon sistemi, `content` özelliği ile CSS üzerinden de kontrol edilebilir.

## Gelecek İyileştirmeler

1. **Responsive Tasarım**: Mobil cihazlarda daha iyi bir deneyim için responsive tasarım iyileştirmeleri yapılabilir.

2. **Toplu İzin Verme**: Birden fazla kullanıcıya aynı anda izin vermek için toplu işlem özelliği eklenebilir.

3. **Filtreleme Seçenekleri**: Daha gelişmiş filtreleme seçenekleri eklenebilir (örn. rol bazlı, departman bazlı).

## İlgili Dosyalar

- `user-page-permissions.component.html`
- `user-page-permissions.component.scss`
- `user-page-permissions.component.ts`
- `dashboard-permission.service.ts`

# Kullanıcı Sayfa İzinleri Yönetimi

Bu belge, dashboard sayfalarına kullanıcı bazlı erişim izinlerini yönetmek için geliştirilen "Kullanıcı Sayfa İzinleri" özelliğinin teknik detaylarını içermektedir.

## Genel Bakış

Kullanıcı Sayfa İzinleri özelliği, dashboard sayfalarına hangi kullanıcıların erişebileceğini yönetmek için geliştirilmiştir. Bu özellik, iki ana sekme içerir:

1. **Kullanıcılar Sekmesi**: Sistemdeki tüm kullanıcıları listeler ve her kullanıcının yanında "+" butonu ile ilgili sayfaya erişim izni verilebilir.
2. **İzinler Sekmesi**: İlgili sayfaya erişim izni verilmiş kullanıcıları listeler ve izinleri kaldırma imkanı sunar.

## Teknik Yapı

### Bileşenler

- **UserPagePermissionsComponent**: Ana bileşen, kullanıcı ve izin yönetimini sağlar.
- **DashboardPermissionService**: İzin işlemleri için gerekli API çağrılarını yapar.

### Modeller

- **User**: Kullanıcı bilgilerini içerir (id, username, fullName, email, isActive, hasPagePermission).
- **PagePermission**: Sayfa izin bilgilerini içerir (id, userId, pageId, username, fullName, grantedAt).
- **DashboardPage**: Sayfa bilgilerini içerir (id, name, description, route, isActive, requiredPermission).

### Kullanıcı Verileri

Sistem şu anda 2 kullanıcı ile çalışmaktadır:
1. **admin**: Yönetici (sicil: A001)
2. **BT**: Bt yönetici (sicil: B001)

> Not: Kullanıcı verileri, gerçek uygulamada API'den çekilecektir. Şu anda mock veriler kullanılmaktadır.

### Dosya Yapısı

```
frontend/src/app/features/dashboard-management/
├── components/
│   ├── user-page-permissions.component.ts
│   ├── user-page-permissions.component.html
│   └── user-page-permissions.component.scss
├── services/
│   └── dashboard-permission.service.ts
└── dashboard-management.routes.ts
```

## Kullanılan PrimeNG Bileşenleri

- **TabView**: Kullanıcılar ve İzinler sekmeleri için
- **Table**: Kullanıcı ve izin listelerini göstermek için
- **Button**: İşlem butonları için
- **Toast**: Bildirimler için
- **InputText**: Arama alanları için
- **Tag**: Durum etiketleri için

## Geliştirme Süreci

### 1. Bileşen ve Servis Oluşturma

İlk olarak, kullanıcı sayfa izinleri için gerekli bileşen ve servis dosyaları oluşturuldu:

- `user-page-permissions.component.ts`
- `user-page-permissions.component.html`
- `user-page-permissions.component.scss`
- `dashboard-permission.service.ts`

### 2. Routing Yapılandırması

Dashboard yönetim modülünün route yapılandırmasına yeni bir route eklendi:

```typescript
{
  path: 'permissions/:id',
  component: UserPagePermissionsComponent,
  canActivate: [AuthGuard, PermissionGuard],
  data: { 
    requiredPermission: 'Pages.DashboardManagement',
    breadcrumb: 'Sayfa İzinleri'
  }
}
```

### 3. Dashboard Yönetim Bileşenine İzinler Butonu Ekleme

Dashboard yönetim bileşeninin HTML dosyasına, her sayfa için izinleri yönetmek üzere bir buton eklendi:

```html
<button pButton pRipple icon="pi pi-users" class="p-button-rounded p-button-text p-button-success dashboard-permissions-icon" (click)="managePermissions(page)" pTooltip="İzinleri Yönet" tooltipPosition="top"></button>
```

### 4. Servis Entegrasyonu

Kullanıcı sayfa izinleri bileşeni, DashboardPermissionService ile entegre edildi. Bu servis, aşağıdaki işlevleri sağlar:

- Sayfa detaylarını getirme
- Tüm kullanıcıları getirme
- Sayfa izinlerini getirme
- Kullanıcıya izin verme
- Kullanıcının iznini kaldırma

## Karşılaşılan Sorunlar ve Çözümleri

### Sorun 1: PowerShell'de && Operatörü Hatası

**Sorun**: PowerShell'de `cd frontend && ng serve` komutu çalıştırıldığında, "&&" operatörünün geçerli bir ifade ayırıcı olmadığı hatası alındı.

**Çözüm**: PowerShell'de komutları ayrı ayrı çalıştırmak gerekiyor:

```powershell
cd frontend
ng serve
```

### Sorun 2: Kullanıcı İzinlerinin Güncellenmesi

**Sorun**: Kullanıcıya izin verildiğinde veya kaldırıldığında, kullanıcı listesindeki izin durumlarının güncellenmesi gerekiyordu.

**Çözüm**: `updateUserPermissions` metodu oluşturuldu. Bu metod, izinler listesinden kullanıcı ID'lerini alarak kullanıcıların izin durumlarını güncelliyor.

```typescript
updateUserPermissions(): void {
  const userIdsWithPermission = this.pagePermissions.map(p => p.userId);
  
  this.users.forEach(user => {
    user.hasPagePermission = userIdsWithPermission.includes(user.id);
  });
  
  this.filteredUsers = [...this.users];
}
```

## Gelecek Geliştirmeler

1. **Toplu İzin Verme**: Birden fazla kullanıcıya aynı anda izin verme özelliği eklenebilir.
2. **İzin Geçmişi**: Kullanıcılara verilen izinlerin geçmişini görüntüleme özelliği eklenebilir.
3. **İzin Süresi**: İzinlere süre sınırlaması getirilebilir (örn. belirli bir tarihte sona eren izinler).
4. **Rol Bazlı İzinler**: Kullanıcı bazlı izinlerin yanı sıra rol bazlı izinler de eklenebilir.

## Sonuç

Kullanıcı Sayfa İzinleri özelliği, dashboard sayfalarına erişim izinlerini kullanıcı bazında yönetmek için etkili bir çözüm sunmaktadır. Kullanıcı dostu arayüzü ve esnek yapısı sayesinde, sistem yöneticileri kolayca sayfa izinlerini yönetebilmektedir. 