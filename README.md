# Stok Yönetim Sistemi

## Proje Hakkında

Bu proje, modern bir stok yönetim sistemi geliştirmeyi amaçlamaktadır. Sistem, stok takibi, sipariş yönetimi, kullanıcı yönetimi ve raporlama gibi temel işlevleri içerir.

## Teknolojiler

- **Backend**: ASP.NET Core 9.0
- **Frontend**: Angular 19
- **Veritabanı**: PostgreSQL
- **UI Kütüphanesi**: PrimeNG
- **Stil**: SASS

## Başlangıç

### Gereksinimler

- .NET 9.0 SDK
- Node.js 20.x veya üzeri
- npm 10.x veya üzeri
- PostgreSQL 16.x

### Kurulum

1. Repoyu klonlayın:
   ```
   git clone https://github.com/kullanici/stok-yonetim.git
   cd stok-yonetim
   ```

2. Backend bağımlılıklarını yükleyin:
   ```
   cd backend
   dotnet restore
   ```

3. Frontend bağımlılıklarını yükleyin:
   ```
   cd ../frontend
   npm install
   ```

4. Veritabanını oluşturun:
   ```
   cd ../backend
   dotnet ef database update
   ```

### Çalıştırma

1. Backend'i çalıştırın:
   ```
   cd backend
   dotnet run
   ```

2. Frontend'i çalıştırın:
   ```
   cd frontend
   npm run start
   ```

3. Tarayıcınızda `http://localhost:4200` adresine gidin.

## Proje Yapısı

```
stok-yonetim/
├── backend/                # ASP.NET Core API
│   ├── Controllers/        # API kontrolcüleri
│   ├── Models/             # Veri modelleri
│   ├── Services/           # İş mantığı servisleri
│   └── Data/               # Veritabanı bağlantısı ve yapılandırması
├── frontend/               # Angular uygulaması
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/       # Çekirdek modüller
│   │   │   ├── features/   # Özellik modülleri
│   │   │   ├── shared/     # Paylaşılan bileşenler
│   │   │   └── app.module.ts
│   │   ├── assets/         # Statik dosyalar
│   │   └── environments/   # Ortam yapılandırmaları
│   └── package.json
├── knowledge-base/         # Proje belgeleri
│   ├── feature_modules/    # Özellik modülleri belgeleri
│   ├── technical/          # Teknik belgeler
│   └── guides/             # Kullanım kılavuzları
├── errors.md               # Hata kayıtları ve çözümleri
└── README.md               # Proje açıklaması
```

## Belgelendirme

Proje ile ilgili tüm belgeler `knowledge-base` klasöründe bulunmaktadır:

- [Belge Yönetim Kuralları](knowledge-base/document-management-rules.mdc)
- [Kullanıcı Sayfa İzinleri Modülü](knowledge-base/feature_modules/user_page_permissions_module.md)
- [Kullanıcı Sayfa İzinleri Bilgi Tabanı](knowledge-base/user_page_permissions_knowledge_base.md)
- [Hata Kayıtları ve Çözümleri](errors.md)
- [Scratchpad - Çalışma Notları](scratchpad.md)

## Teknik Belgeler

- [Sistem Başlatma Kılavuzu](knowledge-base/system_startup_guide.md)
- [Entity Framework Core Namespace Çakışması Çözümü ve Geçiş Kılavuzu](knowledge-base/namespace-migration-guide.md)
- [CQRS Pattern: Kullanıcı Yönetimi](knowledge-base/architectural_patterns/cqrs_user_management.md)
- [CQRS Pattern: Rol Yönetimi](knowledge-base/architectural_patterns/cqrs_role_management.md)
- [CQRS Pattern: Ürün Yönetimi](knowledge-base/architectural_patterns/cqrs_product_management.md)
- [DDD Pattern: Product Entity Uygulaması](knowledge-base/architectural_patterns/ddd_product_entity.md)
- [DDD Pattern: User Entity Yaklaşımı Değişikliği](knowledge-base/architectural_patterns/ddd_user_entity.md)

## Katkıda Bulunma

1. Bu repoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Bir Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## İletişim

Proje Yöneticisi - [email@example.com](mailto:email@example.com)

Proje Linki: [https://github.com/kullanici/stok-yonetim](https://github.com/kullanici/stok-yonetim)
