# Loglama Sistemi Knowledge Base

## Genel Bakış
Bu dokümantasyon, sistemin loglama ve hata yakalama mekanizmalarının nasıl geliştirildiğini, karşılaşılan sorunları ve çözüm yöntemlerini içerir.

## Kullanılan Teknolojiler
- Serilog (Yapılandırılmış loglama için)
- PostgreSQL (Log kayıtlarının saklanması için)
- PrimeNG Toast (Frontend hata gösterimi için)

## Backend Loglama Sistemi

### 1. Global Exception Handler
- Tüm API endpoint'lerinde oluşabilecek hataları merkezi olarak yakalayacak middleware
- Hata detaylarını loglama ve istemciye uygun hata mesajı döndürme
- Stack trace bilgisi eklenerek hata kaynağının daha kolay tespit edilmesi

### 2. Serilog Yapılandırması
- Logların hem dosyaya hem de veritabanına yazılması
- Farklı log seviyeleri için farklı davranışlar (Error, Warning, Information)
- Hassas verilerin loglanması development ortamında aktif

### 3. Audit Logging
- Kullanıcı aktivitelerinin kaydedilmesi
- Kritik işlemlerin izlenmesi
- Audit loglar için ayrı tablo kullanımı

## Frontend Hata Yönetimi

### 1. Global Error Handler
- Angular'ın yerleşik ErrorHandler'ının özelleştirilmesi
- Beklenmeyen hataların yakalanması

### 2. HTTP Interceptor
- API isteklerinde oluşan hataların yakalanması
- Kullanıcı dostu hata mesajlarının gösterilmesi

## Implementasyon Süreci (16.02.2025)

### 1. Serilog Entegrasyonu
- NuGet Paketleri:
  - Serilog.AspNetCore
  - Serilog.Sinks.PostgreSQL
  - Serilog.Sinks.File
  - Serilog.Enrichers.Environment
  - Serilog.Enrichers.Thread

### 2. Planlanan İş Adımları
1. Serilog Kurulumu 
   - Program.cs'de Serilog yapılandırması
   - appsettings.json'da log ayarları
   - PostgreSQL log tablosu migration'ı

2. Global Exception Middleware 
   - ExceptionMiddleware sınıfı
   - ApiException modeli
   - Hata tipleri için extension'lar
   - Stack trace bilgisi eklenmesi

3. Audit Logging 
   - IAuditLogger interface'i
   - AuditLogService implementasyonu
   - Controller action'larına audit log eklenmesi
   - BaseEntity ile oluşturma/güncelleme takibi

### 3. Dikkat Edilecek Noktalar
- Log yazma işlemleri asenkron olmalı
- Exception middleware'de hassas bilgiler filtrelenmeli
- Audit loglar için ayrı tablo kullanılmalı
- Veritabanı bağlantısı havuzu doğru yapılandırılmalı

### 4. Test Stratejisi
1. Unit Testler
   - Exception middleware testleri
   - Audit logging testleri
   - Log format testleri

2. Entegrasyon Testleri
   - PostgreSQL sink testleri
   - Performans testleri
   - Concurrent log yazma testleri

## İşlem Günlüğü

### 1. Serilog Paketlerinin Eklenmesi (16.02.2025 22:12)
 Aşağıdaki NuGet paketleri başarıyla eklendi:
- Serilog.AspNetCore
- Serilog.Sinks.PostgreSQL
- Serilog.Sinks.File
- Serilog.Enrichers.Environment
- Serilog.Enrichers.Thread

**Test:** Paketler başarıyla yüklendi, proje derlemesi başarılı.

### 2. Serilog Yapılandırması (16.02.2025 22:13)
 Serilog yapılandırması tamamlandı:
1. appsettings.json'a Serilog bölümü eklendi:
   - File sink (günlük dönen log dosyaları)
   - PostgreSQL sink (veritabanı logları)
   - Log zenginleştirmeleri (MachineName, ThreadId)
   - Özel log seviyeleri ve override'lar

2. Program.cs güncellendi:
   - Yapılandırma appsettings.json'dan okunacak şekilde değiştirildi

### 3. GlobalExceptionHandler Geliştirmesi (16.02.2025 23:05)
 GlobalExceptionHandler güncellemeleri:
1. Stack trace bilgisi eklendi
2. Hata mesajları daha detaylı hale getirildi
3. Loglama seviyesi ayarlandı

### 4. BaseEntity ve SeedData Düzeltmeleri (16.02.2025 23:15)
 Yapılan değişiklikler:
1. BaseEntity'de constructor eklendi:
   - CreatedAt alanı otomatik set edildi
   - IsDeleted varsayılan false olarak ayarlandı

2. SeedData sınıfı güncellendi:
   - Context oluşturma yöntemi düzeltildi
   - Scope yönetimi iyileştirildi

## Karşılaşılan Sorunlar ve Çözümleri

### 1. Veritabanı Bağlantı Hatası
**Sorun:** Veritabanı bağlantısı sırasında "database does not exist" hatası.

**Çözüm:**
1. Veritabanı manuel olarak oluşturuldu
2. Migration'lar yeniden uygulandı

### 2. Entity Takibi
**Sorun:** Entity'lerin oluşturma ve güncelleme zamanları doğru şekilde ayarlanmıyordu.

**Çözüm:**
1. BaseEntity sınıfına constructor eklendi
2. CreatedAt ve IsDeleted alanları constructor'da ayarlandı

### 3. Context Oluşturma
**Sorun:** SeedData'da context oluşturma yöntemi hatalıydı.

**Çözüm:**
1. IServiceProvider.CreateScope() kullanıldı
2. Context DI container üzerinden alındı
