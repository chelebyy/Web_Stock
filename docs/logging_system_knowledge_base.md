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

### 2. Serilog Yapılandırması
- Logların hem dosyaya hem de veritabanına yazılması
- Farklı log seviyeleri için farklı davranışlar (Error, Warning, Information)

### 3. Audit Logging
- Kullanıcı aktivitelerinin kaydedilmesi
- Kritik işlemlerin izlenmesi

## Frontend Hata Yönetimi

### 1. Global Error Handler
- Angular'ın yerleşik ErrorHandler'ının özelleştirilmesi
- Beklenmeyen hataların yakalanması

### 2. HTTP Interceptor
- API isteklerinde oluşan hataların yakalanması
- Kullanıcı dostu hata mesajlarının gösterilmesi

## Karşılaşılan Hatalar ve Çözümleri

### 1. Serilog PostgreSQL Sink Yapılandırması
**Hata:** Serilog'un PostgreSQL sink'i için tablo oluşturma hatası
**Çözüm:** 
- Migration ile log tablosunun manuel oluşturulması
- Sink yapılandırmasında şema adının belirtilmesi

### 2. Circular Dependency Hatası
**Hata:** LogService ve AuthService arasında circular dependency
**Çözüm:**
- LogService'in constructor injection yerine IServiceProvider kullanması
- Lazy loading pattern'in uygulanması

### 3. Frontend Toast Gösterim Problemi
**Hata:** Aynı anda birden fazla toast mesajının üst üste binmesi
**Çözüm:**
- Toast servisine queue mekanizması eklenmesi
- Mesajların sırayla gösterilmesi

## En İyi Pratikler

1. Log Seviyeleri:
   - ERROR: Uygulama akışını bozan hatalar
   - WARNING: Potansiyel problemler
   - INFO: Önemli işlem bildirimleri
   - DEBUG: Geliştirme aşamasında kullanılacak detaylı bilgiler

2. Audit Logging:
   - Kullanıcı kimliği
   - İşlem tarihi ve saati
   - İşlem tipi
   - Etkilenen veriler
   - IP adresi

3. Hata Mesajları:
   - Kullanıcı dostu mesajlar
   - Teknik detayların sadece loglarda tutulması
   - Güvenlik açığı oluşturabilecek bilgilerin gizlenmesi

## Bakım ve İzleme

1. Log Rotasyonu:
   - Günlük log dosyalarının oluşturulması
   - Eski logların arşivlenmesi
   - Disk alanı yönetimi

2. Performans İzleme:
   - Log yazma işlemlerinin asenkron yapılması
   - Batch log yazma için buffer kullanımı
   - Kritik olmayan logların düşük öncelikli işlenmesi

## Güvenlik Önlemleri

1. Hassas Veri Yönetimi:
   - Kişisel verilerin maskelenmesi
   - Şifre ve token bilgilerinin loglanmaması
   - IP adreslerinin anonimleştirilmesi

2. Log Güvenliği:
   - Log dosyalarına erişim kısıtlaması
   - Log verilerinin şifrelenmesi
   - Audit trail bütünlüğünün korunması
