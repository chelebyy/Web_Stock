# Revir (Sağlık Ünitesi) Feature Modülü (Planlanan)

## Genel Bakış

Revir Feature Modülü, kurumdaki sağlık hizmetleri biriminin yönetimini, hasta kayıtlarının tutulmasını, muayene ve tedavi süreçlerinin takibini sağlayan bir Angular 19 feature modülüdür. **Bu modül, Angular 19 geçişi tamamlandıktan sonra geliştirilecektir.**

## Planlanan Modül Yapısı

```
frontend/src/app/features/revir/
├── components/
│   ├── revir.component.ts (Ana bileşen)
│   ├── patient-list/
│   │   ├── patient-list.component.ts
│   │   ├── patient-list.component.html
│   │   └── patient-list.component.scss
│   ├── patient-detail/
│   │   ├── patient-detail.component.ts
│   │   ├── patient-detail.component.html
│   │   └── patient-detail.component.scss
│   ├── examination/
│   │   ├── examination.component.ts
│   │   ├── examination.component.html
│   │   └── examination.component.scss
│   ├── medicine-inventory/
│   │   ├── medicine-inventory.component.ts
│   │   ├── medicine-inventory.component.html
│   │   └── medicine-inventory.component.scss
│   └── reports/
│       ├── reports.component.ts
│       ├── reports.component.html
│       └── reports.component.scss
├── services/
│   ├── patient.service.ts
│   ├── examination.service.ts
│   ├── medicine.service.ts
│   └── report.service.ts
├── models/
│   ├── patient.model.ts
│   ├── examination.model.ts
│   ├── medicine.model.ts
│   └── filter.model.ts
└── revir.routes.ts
```

## Planlanan Bileşenler

### RevirComponent

**Amaç**: Revir modülünün ana bileşeni, temel işlevlere erişim sağlayacak.

**Özellikler**:
- Hasta kayıtları, muayeneler, ilaç envanteri gibi kategorilere erişim
- Özet sağlık istatistikleri ve durum bilgileri
- Günlük/haftalık/aylık hasta sayıları
- Hızlı erişim bağlantıları

**Route**: `/revir`

### PatientListComponent

**Amaç**: Hasta kayıtlarını listelemek, yeni hasta kaydetmek ve mevcut kayıtları düzenlemek.

**Özellikler**:
- Hasta listeleme ve filtreleme
- Yeni hasta kaydı oluşturma
- Hasta bilgilerini düzenleme
- Hasta geçmişini görüntüleme

**Route**: `/revir/patients`

### PatientDetailComponent

**Amaç**: Hasta detay bilgilerini görüntüleme ve düzenleme.

**Özellikler**:
- Hasta kişisel bilgileri
- Sağlık geçmişi
- Muayene geçmişi
- İlaç kullanım geçmişi

**Route**: `/revir/patients/:id`

### ExaminationComponent

**Amaç**: Muayene kayıtlarını yönetme ve yeni muayene kaydı oluşturma.

**Özellikler**:
- Muayene kaydı oluşturma
- Tanı girişi
- Reçete düzenleme
- Sevk bilgileri

**Route**: `/revir/examinations`

### MedicineInventoryComponent

**Amaç**: İlaç ve tıbbi malzeme envanterini yönetme.

**Özellikler**:
- İlaç listeleme ve filtreleme
- Stok takibi
- Son kullanma tarihi takibi
- İlaç giriş/çıkış işlemleri

**Route**: `/revir/medicines`

### ReportsComponent

**Amaç**: Sağlık birimi raporları oluşturma.

**Özellikler**:
- Hasta istatistikleri
- Hastalık/tanı dağılımları
- İlaç kullanım raporları
- Periyodik sağlık raporları

**Route**: `/revir/reports`

## Planlanan Servisler

### PatientService

**Amaç**: Hasta kayıtlarıyla ilgili API isteklerini yönetecek.

**Metotlar**:
- `getPatients(filters)`: Filtrelere göre hastaları getirir
- `getPatientById(id)`: Belirli bir hastayı getirir
- `createPatient(patient)`: Yeni hasta kaydı oluşturur
- `updatePatient(patient)`: Hasta bilgilerini günceller
- `searchPatients(query)`: Hasta araması yapar

### ExaminationService

**Amaç**: Muayene kayıtlarıyla ilgili API isteklerini yönetecek.

**Metotlar**:
- `getExaminations(filters)`: Filtrelere göre muayeneleri getirir
- `getExaminationById(id)`: Belirli bir muayene kaydını getirir
- `createExamination(examination)`: Yeni muayene kaydı oluşturur
- `updateExamination(examination)`: Muayene kaydını günceller
- `getPatientExaminations(patientId)`: Belirli bir hastanın muayene geçmişini getirir

### MedicineService

**Amaç**: İlaç ve tıbbi malzeme envanterini yönetecek.

**Metotlar**:
- `getMedicines(filters)`: Filtrelere göre ilaçları getirir
- `getMedicineById(id)`: Belirli bir ilacı getirir
- `createMedicine(medicine)`: Yeni ilaç kaydı oluşturur
- `updateMedicine(medicine)`: İlaç bilgilerini günceller
- `recordMedicineUsage(medicineId, quantity, patientId)`: İlaç kullanımını kaydeder
- `checkExpiredMedicines()`: Son kullanma tarihi geçmiş ilaçları kontrol eder

### ReportService

**Amaç**: Rapor oluşturma işlemlerini yönetecek.

**Metotlar**:
- `generatePatientStatistics(period)`: Hasta istatistikleri oluşturur
- `generateDiagnosisStatistics(period)`: Tanı dağılımı istatistikleri oluşturur
- `generateMedicineUsageReport(period)`: İlaç kullanım raporu oluşturur
- `exportToExcel(reportData)`: Raporu Excel formatında dışa aktarır

## Planlanan API Entegrasyonu

Modül aşağıdaki API endpoint'leri ile entegre çalışacak:

- `GET /api/patients`: Hastaları listeler
- `GET /api/patients/{id}`: Belirli bir hastanın detaylarını getirir
- `POST /api/patients`: Yeni hasta kaydı oluşturur
- `PUT /api/patients/{id}`: Hasta bilgilerini günceller
- `GET /api/examinations`: Muayeneleri listeler
- `POST /api/examinations`: Yeni muayene kaydı oluşturur
- `GET /api/medicines`: İlaçları listeler
- `POST /api/medicines/usage`: İlaç kullanımını kaydeder
- `GET /api/reports/patients`: Hasta istatistikleri raporu oluşturur
- `GET /api/reports/diagnoses`: Tanı dağılımı raporu oluşturur

## Lazy Loading Planı

Modül, lazy loading prensibine göre yapılandırılacak ve app.routes.ts içinde şu şekilde tanımlanacak:

```typescript
{
  path: '',
  loadChildren: () => import('./features/revir/revir.routes').then(m => m.REVIR_ROUTES)
}
```

revir.routes.ts dosyası şu rotaları tanımlayacak:

```typescript
export const REVIR_ROUTES: Routes = [
  {
    path: 'revir',
    component: RevirComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
  },
  {
    path: 'revir/patients',
    component: PatientListComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
  },
  {
    path: 'revir/patients/:id',
    component: PatientDetailComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
  },
  {
    path: 'revir/examinations',
    component: ExaminationComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
  },
  {
    path: 'revir/medicines',
    component: MedicineInventoryComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
  },
  {
    path: 'revir/reports',
    component: ReportsComponent,
    canActivate: [AuthGuard, PermissionGuard],
    data: { permission: 'Pages.Revir' }
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
   - Hasta listesi, muayene kayıtları ve ilaç envanteri için signal tabanlı state yönetimi
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

## Gizlilik ve Güvenlik Önlemleri

Sağlık verileri hassas bilgiler içerdiğinden, aşağıdaki güvenlik önlemleri alınacaktır:

1. **Veri Şifreleme**
   - Hassas hasta bilgilerinin şifrelenmesi
   - HTTPS kullanımı

2. **Erişim Kontrolü**
   - Rol bazlı erişim kontrolü
   - Yetkilendirme ve kimlik doğrulama kontrolleri

3. **Loglama ve Denetim**
   - Tüm veri erişimleri ve değişikliklerinin loglanması
   - Şüpheli aktivitelerin izlenmesi

4. **Veri Minimizasyonu**
   - Sadece gerekli sağlık verilerinin toplanması
   - Kullanılmayan verilerin anonim hale getirilmesi

## Bağlantılı Belgeler

- [Angular 19 Migration](../angular-19-migration/angular19_migration.md)
- [Errors & Solutions](../../errors.md)

## Not

**Bu modül, Angular 19 geçişi tamamlandıktan sonra geliştirilecektir.** Şu anda planlanma aşamasındadır ve bu belge süreç ilerledikçe güncellenecektir. 