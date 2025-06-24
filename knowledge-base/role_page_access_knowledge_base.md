# Rol ve Sayfa Erişim İzinleri Bilgi Bankası

## Genel Bakış

Bu belge, rol tabanlı erişim kontrolü (RBAC) ve sayfa erişim izinleri konusundaki tüm bilgileri, sorunları ve çözümleri içerir. Uygulamamızdaki izin yapısı, roller ve sayfalar arasındaki ilişkileri ve bunların nasıl yönetileceğini açıklar.

## Rol Tabanlı Erişim Kontrolü

### Temel Kavramlar

1. **Roller**: Kullanıcılara atanan yetkiler grubu (Admin, Kullanıcı)
2. **İzinler**: Belirli işlemler için verilen yetkiler (Okuma, Yazma, Silme)
3. **Sayfa Erişimleri**: Belirli sayfalara erişim için verilen izinler
4. **Hiyerarşi**: Kullanıcı → Rol → İzinler şeklinde bir zincir

### Mimari Yapı

Rol tabanlı erişim kontrolü aşağıdaki temel bileşenlerden oluşur:

1. **Backend Bileşenleri**:
   - `Role` ve `Permission` entity sınıfları
   - `RoleController` ve `PermissionController` controller sınıfları
   - İzin grupları ve rolleri yönetmek için servisler

2. **Frontend Bileşenleri**:
   - Rol yönetimi sayfası (`RoleManagementComponent`)
   - Rol detay sayfası (`RoleDetailComponent`)
   - İzin bileşenleri (`permission-card`, `role-info-card`, vb.)

3. **Erişim Kontrolü Mekanizması**:
   - JWT token'da rolleri ve izinleri taşıma
   - Guard'lar ile rota bazlı erişim kontrolü
   - Bileşen bazlı erişim kontrolü direktifleri

## Sayfa Erişim İzinleri

### Sayfa Erişim İzinleri Yapısı

Sayfa erişim izinleri, kullanıcıların hangi sayfalara erişebileceğini belirleyen izinlerdir. Backend'de tanımlanan izinler, genellikle "Sayfa Erişimi" veya "PageAccess" grubu altında toplanır ve frontend'de özel bir bölümde gösterilir.

```
SAYFA ERİŞİM İZİNLERİ
└── Admin Dashboard
    └── Erişim İzni: Admin Dashboard sayfasına erişim
└── User Dashboard
    └── Erişim İzni: User Dashboard sayfasına erişim
└── Kullanıcı Yönetimi
    └── Erişim İzni: Kullanıcı yönetimi sayfasına erişim
└── Rol Yönetimi
    └── Erişim İzni: Rol yönetimi sayfasına erişim
```

### Sayfa Erişim İzinleri UI Bileşeni

Rol detay sayfasında, sayfa erişim izinleri özel bir bölümde gösterilir. Bu bölüm, ToggleButton bileşenleri ile sayfalar için izinleri yönetmeyi sağlar:

```html
<div class="permission-card">
  <h3>Sayfa Erişim İzinleri</h3>
  <p>Bu rolün hangi sayfalara erişebileceğini ayarlayın</p>
  
  <div class="permission-grid">
    <div *ngFor="let permission of getPermissionsByGroup('Sayfa Erişimi')" class="permission-item">
      <p-toggleButton 
        [ngModel]="isPermissionSelected(permission.id)"
        (onChange)="togglePermission(permission.id)"
        onLabel="Açık" 
        offLabel="Kapalı" 
        styleClass="permission-toggle">
      </p-toggleButton>
      <div class="permission-details">
        <h4>{{ permission.name }}</h4>
        <p>{{ permission.description }}</p>
      </div>
    </div>
  </div>
</div>
```

## Karşılaşılan Sorunlar ve Çözümleri

### 1. İzin Grubu Bulunamama Sorunu

**Sorun**: "Sayfa Erişimi" grubu altındaki izinler gösterilemiyor, çünkü backend'den gelen API yanıtında "Sayfa Erişimi" adında bir grup bulunmuyor.

**Nedeni**: Backend'de izinler farklı bir grup adı altında tutuluyor olabilir (örn. "Page", "Pages", "PageAccess").

**Çözüm**: `getPermissionsByGroup` metodunda, "Sayfa Erişimi" için olası grup adlarını kontrol eden bir mekanizma ekledik:

```typescript
getPermissionsByGroup(groupName: string): Permission[] {
  if (!this.permissionGroups || !Array.isArray(this.permissionGroups)) {
    console.warn('permissionGroups dizi değil veya tanımsız');
    return [];
  }
  
  // Sayfa Erişimi grubu özel durumu
  if (groupName === 'Sayfa Erişimi') {
    // Backenddeki muhtemel sayfa erişim grupları
    const possiblePageGroups = ['Page', 'Pages', 'PageAccess', 'PagePermissions', 'Navigation'];
    
    // Mevcut grup adlarıyla olası sayfa gruplarını karşılaştır
    for (const possibleName of possiblePageGroups) {
      const matchingGroup = this.permissionGroups.find(g => g && g.group === possibleName);
      if (matchingGroup) {
        console.log(`"Sayfa Erişimi" için eşleşen grup bulundu: "${possibleName}"`);
        return matchingGroup.permissions;
      }
    }
    
    // Özel bir eşleşme bulunamadı, o zaman "Page" içeren herhangi bir grup var mı diye bakalım
    const pageGroup = this.permissionGroups.find(g => 
      g && g.group && (g.group.includes('Page') || g.group.includes('page') || g.group.includes('Sayfa'))
    );
    
    if (pageGroup) {
      console.log(`"Sayfa Erişimi" için kısmi eşleşen grup bulundu: "${pageGroup.group}"`);
      return pageGroup.permissions;
    }
    
    // Eğer hiçbir eşleşme bulunamazsa, ilk grubu kullan
    if (this.permissionGroups.length > 0 && this.permissionGroups[0]) {
      console.log(`"Sayfa Erişimi" için grup bulunamadı, ilk grup kullanılıyor: "${this.permissionGroups[0].group}"`);
      return this.permissionGroups[0].permissions;
    }
  }
  
  // Normal grup arama
  const group = this.permissionGroups.find(g => g && g.group === groupName);
  return group && group.permissions ? group.permissions : [];
}
```

### 2. ReferenceHandler.Preserve Formatı Sorunu

**Sorun**: Backend API'den gelen yanıtlarda, izinler ve izin grupları normal dizi yerine `{ $id: '1', $values: [...] }` şeklinde bir formatta geliyor.

**Çözüm**: İzin işleme kodunu, ReferenceHandler.Preserve formatını destekleyecek şekilde güncelledik:

```typescript
// PreserveFormat<T> tipini tanımla
interface PreserveFormat<T> {
  $id?: string;
  $values?: T[];
}

// İzinleri kontrol ederken bu formatı dikkate al
if (Array.isArray(group.permissions)) {
  return group.permissions;
} else if (group.permissions && typeof group.permissions === 'object') {
  // PreserveFormat kontrolü
  const preserveData = group.permissions as unknown as PreserveFormat<Permission>;
  return preserveData && preserveData.$values ? preserveData.$values : [];
}
```

### 3. ToggleButton Veri Bağlama Sorunları

**Sorun**: PrimeNG ToggleButton bileşeni için `[checked]` ve `[value]` özelliklerinin kullanılması Angular hatalarına neden oldu.

**Çözüm**: ToggleButton bileşenini doğrudan izin ID'sini kontrol eden, özel bir mekanizma ile güncelledik:

```html
<!-- Hatalı kullanım -->
<p-toggleButton 
  [checked]="isPermissionSelected(permission.id)" 
  [(ngModel)]="selectedPermissions"
  [value]="permission.id">
</p-toggleButton>

<!-- Doğru kullanım -->
<p-toggleButton 
  [ngModel]="isPermissionSelected(permission.id)"
  (onChange)="togglePermission(permission.id)"
  onLabel="Açık" 
  offLabel="Kapalı">
</p-toggleButton>
```

```typescript
// İzin seçili mi kontrolü
isPermissionSelected(permissionId: number): boolean {
  return this.selectedPermissions.includes(permissionId);
}

// İzin ekle/çıkar
togglePermission(permissionId: number): void {
  const index = this.selectedPermissions.indexOf(permissionId);
  if (index > -1) {
    this.selectedPermissions.splice(index, 1);
  } else {
    this.selectedPermissions.push(permissionId);
  }
}
```

### 4. API Endpoint Uyumsuzluğu

**Sorun**: `api/roles` endpoint'ine yapılan isteklerde 404 (Not Found) hatası alındı.

**Nedeni**: Backend controller'ın adı `RoleController` olduğu için ASP.NET Core endpoint'i `api/Role` olarak oluşturuyor.

**Çözüm**: RoleService'de API URL'sini düzelttik:

```typescript
// Hatalı
private apiUrl = `${environment.apiUrl}/api/roles`;

// Doğru
private apiUrl = `${environment.apiUrl}/api/Role`;
```

### 5. Döngüsel Referans Serileştirme Hatası

**Sorun**: Rolleri çekerken 500 Internal Server Error (döngüsel referans hatası) alındı.

**Çözüm**: Backend Program.cs dosyasında JSON serileştirme ayarları değiştirildi:

```csharp
// Hatalı
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

// Doğru
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
```

Ayrıca frontend kodunda `normalizeResponse` metodu uygulandı.

## Yeni İzinleri Ekleme

Yeni bir sayfa için izin eklemek isterseniz, aşağıdaki adımları izleyin:

1. **Backend'de SeedData'ya İzin Ekleme**:

```csharp
// SeedData.cs içinde
private async Task SeedPermissionsAsync(ApplicationDbContext context)
{
    var permissions = new List<Permission>
    {
        // Mevcut izinler...
        
        // Yeni sayfa izinleri
        new Permission { Name = "Pages.NewPage", Description = "Yeni Sayfa erişim izni", Group = "PageAccess" },
    };
    
    // ...
}
```

2. **Frontend Route'unda İzin Kontrolü Ekleme**:

```typescript
// app.routes.ts içinde
{
  path: 'new-page',
  component: NewPageComponent,
  canActivate: [AuthGuard],
  data: {
    requiresAdmin: false,
    requiredPermission: 'Pages.NewPage'
  }
}
```

3. **AuthGuard'da İzin Kontrolünü Uygulama**:

```typescript
canActivate(route: ActivatedRouteSnapshot): boolean {
  const requiredPermission = route.data?.['requiredPermission'] as string;
  
  if (requiredPermission && !this.authService.hasPermission(requiredPermission)) {
    console.log(`Erişim reddedildi: "${requiredPermission}" izni gerekiyor`);
    this.router.navigate(['/access-denied']);
    return false;
  }
  
  return true;
}
```

## İzin Grubu ve Rol Yapısı

### İzin Grupları

Sistemde şu izin grupları bulunur:

1. **PageAccess (Sayfa Erişimi)**: Sayfaların erişimi için izinler
2. **UserManagement (Kullanıcı Yönetimi)**: Kullanıcıları yönetmek için izinler
3. **RoleManagement (Rol Yönetimi)**: Rolleri yönetmek için izinler
4. **SystemSettings (Sistem Ayarları)**: Sistem ayarlarını yönetmek için izinler

### Roller

Sistemde varsayılan olarak iki rol bulunur:

1. **Admin**: Tüm izinlere sahiptir
2. **Kullanıcı**: Sadece belirli izinlere sahiptir

## İyileştirme Önerileri

1. **Merkezi İzin Yönetimi**: Tüm izinleri merkezi bir şekilde yönetmek için bir servis oluşturun

2. **İzin Gruplarını Çevirme**: İzin gruplarını ve isimlerini kullanıcı dostu hale getirmek için çeviri desteği ekleyin

3. **Görsel İyileştirmeler**:
   - İzin grupları için simgeler ekleyin
   - İzin açıklamalarını daha detaylı hale getirin
   - Toggle butonları için renk kodlaması ekleyin

4. **Toplu İşlemler**:
   - Tüm izinleri seçme/kaldırma butonu ekleyin
   - Grup bazında izinleri seçme/kaldırma butonu ekleyin
   - Varsayılan rol şablonları oluşturun

5. **Erişim Kontrolü İzleme**: Sayfalara ve işlemlere erişim denemeleri için izleme ve loglama ekleyin

## Referans Mimari

Frontend rol ve izin yönetiminin mimarisi şu şekildedir:

```
services/
  |- auth.service.ts (token ve izin kontrolü)
  |- role.service.ts (rol işlemleri)
  |- permission.service.ts (izin işlemleri)
  
models/
  |- role.model.ts (rol modeli)
  |- permission.model.ts (izin modeli)
  
components/
  |- role-management/
     |- role-management.component.ts (rol listesi)
     |- role-detail/
        |- role-detail.component.ts (rol detayı ve izinleri)
  
guards/
  |- auth.guard.ts (rota bazlı izin kontrolü)
  
directives/
  |- has-permission.directive.ts (elemana özel izin kontrolü)
``` 