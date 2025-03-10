# Stok Yönetim Sistemi Projesi

## Genel Bakış

Bu proje, bir kurumun envanter ve stok takibini yönetmek için geliştirilmiş bir web uygulamasıdır. Sistem, kullanıcı yönetimi, rol ve izin yönetimi, bilgi işlem envanteri ve sağlık ünitesi modüllerini içerir.

## Teknoloji Stack

### Backend
- .NET Core 9
- PostgreSQL 17.3
- Entity Framework Core
- Clean Architecture

### Frontend
- Angular 19.1.2
- PrimeNG 19.0.9
- Tailwind CSS 3.4.1
- RxJS

## Proje Yapısı

```
proje/
├── src/                        # Backend kaynak kodları
│   ├── Stock.API/              # API katmanı
│   ├── Stock.Application/      # Uygulama katmanı
│   ├── Stock.Domain/           # Domain katmanı
│   └── Stock.Infrastructure/   # Altyapı katmanı
├── frontend/                   # Frontend kaynak kodları
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/           # Çekirdek bileşenler ve hizmetler
│   │   │   ├── features/       # Feature modülleri
│   │   │   ├── shared/         # Paylaşılan bileşenler ve servisler
│   │   │   └── models/         # Veri modelleri
│   │   ├── assets/             # Statik dosyalar
│   │   └── environments/       # Ortam yapılandırmaları
│   └── angular.json
├── knowledge-base/             # Proje belgeleri ve bilgi tabanı
│   ├── feature_modules/        # Feature modülleri belgeleri
│   ├── angular-19-migration/   # Angular 19 geçiş belgeleri
│   └── ...                     # Diğer belgeler
├── scratchpad.md               # Aktif çalışma notları ve görev takibi
├── errors.md                   # Hata kayıtları ve çözümleri
└── README.md                   # Bu dosya
```

## Belge Organizasyonu

### Temel Belgeler

- **README.md** (Bu dosya): Proje genel bakışı ve belge organizasyonu
- **scratchpad.md**: Aktif çalışma notları, tamamlanan görevler ve planlanan görevler
- **errors.md**: Karşılaşılan hatalar ve çözüm yöntemleri

### Knowledge Base

Proje ile ilgili detaylı bilgiler ve teknik dokümanlar `knowledge-base/` dizininde bulunur:

#### Feature Modülleri

- [`knowledge-base/feature_modules/role_management_module.md`](knowledge-base/feature_modules/role_management_module.md): Rol Yönetimi Modülü belgeleri
- [`knowledge-base/feature_modules/user_management_module.md`](knowledge-base/feature_modules/user_management_module.md): Kullanıcı Yönetimi Modülü belgeleri
- [`knowledge-base/feature_modules/bilgi_islem_module.md`](knowledge-base/feature_modules/bilgi_islem_module.md): Bilgi İşlem Modülü belgeleri (Angular 19 geçişi sonrası)
- [`knowledge-base/feature_modules/revir_module.md`](knowledge-base/feature_modules/revir_module.md): Revir (Sağlık Ünitesi) Modülü belgeleri (Angular 19 geçişi sonrası)

#### Mimari ve Tasarım

- [`knowledge-base/angular-19-migration/angular19_migration.md`](knowledge-base/angular-19-migration/angular19_migration.md): Angular 19 geçiş planı ve ilerleme durumu
- [`knowledge-base/shared_module_knowledge_base.md`](knowledge-base/shared_module_knowledge_base.md): Shared modülü belgeleri
- [`knowledge-base/system_startup_guide.md`](knowledge-base/system_startup_guide.md): Sistem başlatma rehberi

## Başlangıç

### Sistem Gereksinimleri

- .NET 9 SDK
- Node.js 20+
- npm 10+
- PostgreSQL 17+
- Visual Studio/VS Code

### Kurulum Adımları

1. Repoyu klonlayın
   ```
   git clone https://github.com/yourusername/stock-management.git
   cd stock-management
   ```

2. Backend'i başlatın
   ```
   cd src/Stock.API
   dotnet restore
   dotnet run
   ```

3. Frontend'i başlatın
   ```
   cd frontend
   npm install
   npm start
   ```

4. Tarayıcınızda http://localhost:4202 adresine gidin

Daha detaylı bilgi için [`knowledge-base/system_startup_guide.md`](knowledge-base/system_startup_guide.md) dosyasına bakın.

## Katkı Sağlama

1. Bir branch oluşturun (`git checkout -b feature/amazing-feature`)
2. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
3. Branch'inizi push edin (`git push origin feature/amazing-feature`)
4. Pull Request açın

## Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır. 