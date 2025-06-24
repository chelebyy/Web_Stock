# Shared Modülü Knowledge Base

## Shared Modülü Nedir?

Shared modülü, uygulamanın farklı bölümlerinde ortak olarak kullanılan bileşenleri, direktifleri, pipe'ları ve diğer paylaşılan özellikleri içeren bir modüldür. Bu modül, kod tekrarını önler ve uygulamanın bakımını kolaylaştırır.

## Shared Modülü Yapısı

```
src/app/shared/
├── components/       # Paylaşılan bileşenler
├── directives/       # Paylaşılan direktifler
│   └── has-permission.directive.ts
├── pipes/            # Paylaşılan pipe'lar
├── ui/               # Paylaşılan UI bileşenleri
└── index.ts          # Dışa aktarma dosyası
```

## Shared Modülü Oluşturma Adımları

1. **Klasör Yapısını Oluşturma**:
   ```powershell
   mkdir frontend\src\app\shared
   mkdir frontend\src\app\shared\components
   mkdir frontend\src\app\shared\directives
   mkdir frontend\src\app\shared\pipes
   mkdir frontend\src\app\shared\ui
   ```

2. **Direktifleri Taşıma**:
   - has-permission.directive.ts dosyasını frontend/src/app/directives klasöründen frontend/src/app/shared/directives klasörüne taşıma
   - Import yollarını güncelleme:
     ```typescript
     // Eski
     import { AuthService } from '../core/authentication/auth.service';
     
     // Yeni
     import { AuthService } from '../../core/authentication/auth.service';
     ```

3. **Index.ts Dosyası Oluşturma**:
   ```typescript
   // Directives
   export * from './directives/has-permission.directive';
   
   // Pipes
   // export * from './pipes/...';
   
   // Components
   // export * from './components/...';
   
   // UI Components
   // export * from './ui/...';
   ```

4. **Import Yollarını Güncelleme**:
   - app.module.ts dosyasındaki import yolunu güncelleme:
     ```typescript
     // Eski
     import { HasPermissionDirective } from './directives/has-permission.directive';
     
     // Yeni
     import { HasPermissionDirective } from './shared/directives/has-permission.directive';
     ```
   
   - user-dashboard.component.ts dosyasındaki import yolunu güncelleme:
     ```typescript
     // Eski
     import { HasPermissionDirective } from '../../directives/has-permission.directive';
     
     // Yeni
     import { HasPermissionDirective } from '../../shared/directives/has-permission.directive';
     ```

5. **Eski Dosyaları Silme**:
   ```powershell
   rm frontend\src\app\directives\has-permission.directive.ts
   ```

## Karşılaşılan Sorunlar ve Çözümleri

1. **PowerShell Komut Sorunları**:
   - **Sorun**: PowerShell'de `-p` parametresi çalışmıyor.
   - **Çözüm**: Komutları ayrı ayrı çalıştırdık.

## Öğrenilen Dersler

1. **Modüler Yapı Avantajları**:
   - Daha düzenli kod yapısı
   - Daha kolay bakım
   - Daha iyi ölçeklenebilirlik

2. **Angular 19 Best Practices**:
   - Shared modülü kullanımı
   - Standalone bileşenler ve direktifler
   - Dışa aktarma için index.ts dosyası kullanımı

3. **Refactoring İpuçları**:
   - Önce klasör yapısını oluştur
   - Dosyaları taşı
   - Import yollarını güncelle
   - Eski dosyaları sil
   - Derleme ve test yap

## Sonraki Adımlar

1. **Pipe'ları Taşıma**:
   - Pipe'ları shared/pipes klasörüne taşıma
   - Import yollarını güncelleme

2. **Paylaşılan Bileşenleri Taşıma**:
   - Paylaşılan bileşenleri shared/components klasörüne taşıma
   - Import yollarını güncelleme

3. **UI Bileşenlerini Oluşturma**:
   - Paylaşılan UI bileşenlerini shared/ui klasörüne taşıma
   - Import yollarını güncelleme 