# Bilgi İşlem Feature Modülü (Planlanan)

## Genel Bakış

Bilgi İşlem Feature Modülü, kurumun bilgi işlem ekipmanlarının (bilgisayarlar, yazıcılar, ağ ekipmanları, vb.) envanterini yönetmek, arıza kayıtlarını tutmak ve bakım işlemlerini takip etmek için kullanılacak bir Angular 19 feature modülüdür. **Bu modül, Angular 19 geçişi tamamlandıktan sonra geliştirilecektir.**

## Planlanan Modül Yapısı

```
frontend/src/app/features/bilgi-islem/
├── components/
│   ├── bilgi-islem.component.ts (Ana bileşen)
│   ├── equipment-list/
│   │   ├── equipment-list.component.ts
│   │   ├── equipment-list.component.html
│   │   └── equipment-list.component.scss
│   ├── equipment-detail/
│   │   ├── equipment-detail.component.ts
│   │   ├── equipment-detail.component.html
│   │   └── equipment-detail.component.scss
│   ├── service-records/
│   │   ├── service-records.component.ts
│   │   ├── service-records.component.html
│   │   └── service-records.component.scss
│   └── reports/
│       ├── reports.component.ts
│       ├── reports.component.html
│       └── reports.component.scss
├── services/
│   ├── equipment.service.ts
│   ├── service-record.service.ts
│   └── report.service.ts
├── models/
│   ├── equipment.model.ts
│   ├── service-record.model.ts
│   └── filter.model.ts
└── bilgi-islem.routes.ts
```

## Planlanan Bileşenler

### BilgiIslemComponent

**Amaç**: Bilgi İşlem modülünün ana bileşeni, ekipman kategorilerini ve işlevleri sağlayacak.

**Özellikler**:
- Bilgisayar, yazıcı, ağ ekipmanı vb. kategorilere erişim
- Özet istatistikler ve durum bilgileri
- Hızlı erişim bağlantıları

**Route**: `/bilgi-islem`

### EquipmentListComponent

**Amaç**: Ekipmanları listelemek, filtrelemek ve eklemek.

**Özellikler**:
- Ekipman listeleme ve filtreleme
- Ekipman ekleme/düzenleme/silme
- Kategori bazlı görüntüleme
- Duruma göre filtreleme (aktif, arızalı, hurdada vb.)

**Route**: `/bilgi-islem/equipment`

### EquipmentDetailComponent

**Amaç**: Ekipmanın detay bilgilerini görüntüleme ve düzenleme.

**Özellikler**:
- Ekipman detayları (seri no, model, üretici, alım tarihi vb.)
- Bakım geçmişi
- Parça değişim kayıtları
- Zimmet bilgileri

**Route**: `/bilgi-islem/equipment/:id`

### ServiceRecordsComponent

**Amaç**: Ekipmanlara ait bakım ve arıza kayıtlarını yönetme.

**Özellikler**:
- Servis kaydı oluşturma
- Bakım takvimleri
- Arıza raporları
- Teknisyen atama

**Route**: `/bilgi-islem/service-records`

### ReportsComponent

**Amaç**: Bilgi İşlem ekipmanlarına ait raporlar oluşturma.

**Özellikler**:
- Envanter raporları
- Arıza istatistikleri
- Maliyet analizleri
- Grafik ve tablolarla görselleştirme

**Route**: `/bilgi-islem/reports`

## Planlanan Servisler

### EquipmentService

**Amaç**: Ekipmanlarla ilgili API isteklerini yönetecek.

**Metotlar**:
- `getEquipment(filters)`: Filtreleye göre ekipmanları getirir
- `getEquipmentById(id)`: Belirli bir ekipmanı getirir
- `createEquipment(equipment)`: Yeni ekipman oluşturur
- `updateEquipment(equipment)`: Ekipman bilgilerini günceller
- `deleteEquipment(id)`: Ekipman siler
- `assignEquipment(equipmentId, userId)`: Ekipmanı bir kullanıcıya zimmetler

### ServiceRecordService

**Amaç**: Servis kayıtlarıyla ilgili API isteklerini yönetecek.

**Metotlar**:
- `getServiceRecords(filters)`: Filtreleye göre servis kayıtlarını getirir
- `getServiceRecordById(id)`: Belirli bir servis kaydını getirir
- `createServiceRecord(record)`: Yeni servis kaydı oluşturur
- `updateServiceRecord(record)`: Servis kaydını günceller
- `closeServiceRecord(id, resolution)`: Servis kaydını çözüldü olarak işaretler

### ReportService

**Amaç**: Raporlama işlemlerini yönetecek.

**Metotlar**:
- `generateInventoryReport(filters)`: Envanter raporu oluşturur
- `generateFailureStatistics(period)`: Arıza istatistikleri oluşturur
- `generateCostAnalysis(period)`: Maliyet analizi raporu oluşturur
- `exportToExcel(reportData)`: Raporu Excel formatında dışa aktarır

## Planlanan API Entegrasyonu

Modül aşağıdaki API endpoint'leri ile entegre çalışacak:

- `GET /api/equipment`: Ekipmanları listeler
- `GET /api/equipment/{id}`: Belirli bir ekipmanın detaylarını getirir
- `POST /api/equipment`: Yeni ekipman oluşturur
- `PUT /api/equipment/{id}`: Ekipman bilgilerini günceller
- `DELETE /api/equipment/{id}`: Ekipman siler
- `POST /api/equipment/{id}/assign`: Ekipmanı bir kullanıcıya zimmetler
- `GET /api/service-records`: Servis kayıtlarını listeler
- `POST /api/service-records`: Yeni servis kaydı oluşturur
- `GET /api/reports/inventory`: Envanter raporu oluşturur
- `GET /api/reports/failures`: Arıza istatistikleri oluşturur

## Lazy Loading Planı

Modül, lazy loading prensibine göre yapılandırılacak ve app.routes.ts içinde şu şekilde tanımlanacak:

```typescript
{
  path: '',
  loadChildren: () => import('./features/bilgi-islem/bilgi-islem.routes').then(m => m.BILGI_ISLEM_ROUTES)
}
```

bilgi-islem.routes.ts dosyası şu rotaları tanımlayacak:

```typescript
export const BILGI_ISLEM_ROUTES: Routes = [
  {
    path: 'bilgi-islem',
    component: BilgiIslemComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.BilgiIslem' }
  },
  {
    path: 'bilgi-islem/equipment',
    component: EquipmentListComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.BilgiIslem' }
  },
  {
    path: 'bilgi-islem/equipment/:id',
    component: EquipmentDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.BilgiIslem' }
  },
  {
    path: 'bilgi-islem/service-records',
    component: ServiceRecordsComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.BilgiIslem' }
  },
  {
    path: 'bilgi-islem/reports',
    component: ReportsComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.BilgiIslem' }
  }
];
```

## Geliştirme Aşamaları

Bu modül aşağıdaki aşamalarda geliştirilecektir:

1. **Ön Tasarım ve Planlama** (Angular 19 geçişi tamamlandıktan sonra)
   - Kullanıcı gereksinimlerinin detaylandırılması
   - Backend API gereksinimleri
   - Mimari tasarım

2. **Backend Altyapısının Geliştirilmesi**
   - Veri modellerinin oluşturulması
   - API endpoints geliştirilmesi
   - Veritabanı yapılandırması

3. **Frontend Bileşenlerinin Geliştirilmesi**
   - Ana bileşenlerin oluşturulması
   - Servis entegrasyonu
   - UI/UX tasarımı

4. **Test ve Optimizasyon**
   - Birim testleri
   - Entegrasyon testleri
   - Performans optimizasyonu

5. **Kullanıcı Eğitimi ve Belgelendirme**
   - Kullanım kılavuzu
   - Admin belgelendirmesi
   - Eğitim oturumları

## Kullanılacak Modern Angular 19 Özellikleri

Modül aşağıdaki Angular 19 özelliklerini kullanacak:

1. **Signal API**
   - Ekipman listesi, filtreleme ve durum bilgileri için signal tabanlı state yönetimi
   - Computed signal'lar ile dinamik veri dönüşümü
   - `@input()` signal'lar ile bileşenler arası iletişim

2. **Yeni Control Flow Syntax**
   - `@if`, `@for`, `@switch` ile template kodunun sadeleştirilmesi
   - Deferrable views ile büyük listelerin lazy loading'i

3. **Standalone Components**
   - Modül deklarasyonları olmadan direkt bileşen kullanımı
   - İlgili bağımlılıkların imports dizisinde tanımlanması

4. **Angular Signals**
   - RxJS BehaviorSubject'lerin yerine Signal kullanımı
   - Reactive programlama yaklaşımı

## Bağlantılı Belgeler

- [Angular 19 Migration](../angular-19-migration/angular19_migration.md)
- [Bilgi İşlem Modülü Bilgi Tabanı](../bilgi_islem_module_knowledge_base.md)
- [IT Equipment Knowledge Base](../it_equipment_knowledge_base.md)
- [Errors & Solutions](../../errors.md)

## Not

**Bu modül, Angular 19 geçişi tamamlandıktan sonra geliştirilecektir.** Şu anda planlanma aşamasındadır ve bu belge süreç ilerledikçe güncellenecektir.

## Bilgi İşlem Sayfası Erişim İzinleri Düzeltmesi

### Tarih: 11.03.2024

### Sorun
Bilgi İşlem sayfasına erişim izinleri ile ilgili bir tutarsızlık tespit edildi. Kullanıcı dashboard bileşenindeki `checkPermissions` metodu, Bilgi İşlem sayfasına erişim için `IT.Access` izni kontrol ederken, rota tanımında ve HTML şablonunda `Pages.BilgiIslem.View` izni kullanılıyordu. Bu tutarsızlık, bazı kullanıcıların (örneğin B001) doğru izinlere sahip olmasına rağmen Bilgi İşlem sayfasına erişememesine neden oluyordu.

### Yapılan Değişiklikler

1. `frontend/src/app/features/bilgi-islem/bilgi-islem.routes.ts` dosyasında:
   - Rota tanımından `requiresAdmin: true` parametresi kaldırıldı, böylece admin olmayan kullanıcılar da gerekli izinlere sahip olduklarında sayfaya erişebilecekler.
   - İzin adı `Pages.BilgiIslem` yerine `Pages.BilgiIslem.View` olarak güncellendi.

2. `frontend/src/app/features/dashboard/components/user-dashboard.component.ts` dosyasında:
   - `checkPermissions` metodunda Bilgi İşlem sayfası için izin kontrolü `IT.Access` yerine `Pages.BilgiIslem.View` olarak güncellendi.

### Sonuç
Bu değişikliklerle birlikte, kullanıcılar `Pages.BilgiIslem.View` iznine sahip olduklarında Bilgi İşlem sayfasına erişebilecekler. Admin olmayan kullanıcılar da gerekli izinlere sahip olduklarında sayfaya erişebilecekler.

### Öğrenilen Dersler
- İzin kontrollerinin tüm uygulama genelinde tutarlı olması önemlidir.
- Rota tanımları ve bileşen izin kontrolleri arasında uyumsuzluk olmamalıdır.
- Gereksiz admin kısıtlamaları, izin tabanlı erişim kontrolünün etkinliğini azaltabilir. 