# Core Modülü Knowledge Base

## Core Modülü Nedir?

Core modülü, uygulamanın temel servislerini, interceptor'larını ve guard'larını içeren bir modüldür. Bu modül, uygulamanın çekirdeğini oluşturur ve diğer modüller tarafından kullanılan ortak işlevselliği sağlar.

## Core Modülü Yapısı

```
src/app/core/
├── authentication/       # Kimlik doğrulama ile ilgili servisler ve modeller
│   ├── auth.service.ts
│   ├── auth.service.spec.ts
│   └── auth.model.ts
├── http/                # HTTP interceptor'lar
│   └── auth.interceptor.ts
├── guards/              # Route guard'ları
│   ├── auth.guard.ts
│   └── permission.guard.ts
└── services/            # Temel servisler
    ├── log.service.ts
    └── error.service.ts
```

## Core Modülü Oluşturma Adımları

1. **Klasör Yapısını Oluşturma**:
   ```powershell
   mkdir frontend\src\app\core\authentication
   mkdir frontend\src\app\core\http
   mkdir frontend\src\app\core\guards
   mkdir frontend\src\app\core\services
   ```

2. **Servisleri Taşıma**:
   - AuthService -> core/authentication
   - LogService -> core/services
   - ErrorService -> core/services

3. **Modelleri Taşıma**:
   - auth.model.ts -> core/authentication

4. **Interceptor'ları Taşıma**:
   - AuthInterceptor -> core/http

5. **Guard'ları Taşıma**:
   - AuthGuard -> core/guards
   - PermissionGuard -> core/guards

6. **Import Yollarını Güncelleme**:
   - Tüm bileşenlerdeki import yollarını güncelleme
   - app.config.ts ve app.routes.ts dosyalarındaki import yollarını güncelleme

7. **Eski Dosyaları Silme**:
   ```powershell
   rm frontend\src\app\services\auth.service.ts
   rm frontend\src\app\services\auth.service.spec.ts
   rm frontend\src\app\services\log.service.ts
   rm frontend\src\app\services\error.service.ts
   rm frontend\src\app\interceptors\auth.interceptor.ts
   rm frontend\src\app\guards\auth.guard.ts
   rm frontend\src\app\guards\permission.guard.ts
   rm frontend\src\app\models\auth.model.ts
   ```

## Karşılaşılan Sorunlar ve Çözümleri

1. **Import Yolu Hataları**:
   - **Sorun**: Taşınan dosyaların import yolları değiştiği için derleme hataları oluştu.
   - **Çözüm**: Tüm bileşenlerdeki import yolları güncellendi.

2. **PowerShell Komut Sorunları**:
   - **Sorun**: PowerShell'de `&&` operatörü çalışmıyor.
   - **Çözüm**: Komutları `;` ile ayırarak çalıştırdık.

## Öğrenilen Dersler

1. **Modüler Yapı Avantajları**:
   - Daha düzenli kod yapısı
   - Daha kolay bakım
   - Daha iyi ölçeklenebilirlik

2. **Angular 19 Best Practices**:
   - Core modülü kullanımı
   - Singleton servisler için `providedIn: 'root'` kullanımı
   - Standalone bileşenler ve servisler

3. **Refactoring İpuçları**:
   - Önce klasör yapısını oluştur
   - Dosyaları taşı
   - Import yollarını güncelle
   - Eski dosyaları sil
   - Derleme ve test yap

## Sonraki Adımlar

1. **Shared Modülü Oluşturma**:
   - Paylaşılan bileşenleri shared klasörüne taşıma
   - Direktifleri shared klasörüne taşıma
   - Pipe'ları shared klasörüne taşıma

2. **Feature Modülleri Oluşturma**:
   - Her özellik için ayrı modül oluşturma
   - Lazy loading uygulama 