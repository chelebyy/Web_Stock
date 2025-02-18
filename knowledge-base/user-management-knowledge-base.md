# Kullanıcı Yönetimi Knowledge Base

## Genel Bakış
Kullanıcı yönetimi modülü, sistemdeki kullanıcıların oluşturulması, düzenlenmesi ve silinmesi işlemlerini yönetir. Sadece admin yetkisine sahip kullanıcılar bu sayfaya erişebilir.

## API Endpoint'leri

### Auth Controller (/api/auth)
- POST `/api/auth/login`: Kullanıcı girişi
- POST `/api/auth/create-user`: Yeni kullanıcı oluşturma (Sadece admin)
- GET `/api/auth/me`: Mevcut kullanıcı bilgilerini getirme

### User Controller (/api/user)
- GET `/api/user`: Tüm kullanıcıları listeler
- GET `/api/user/{id}`: Belirli bir kullanıcının detaylarını getirir
- PUT `/api/user/{id}`: Kullanıcı bilgilerini günceller
- DELETE `/api/user/{id}`: Kullanıcıyı siler

## Frontend Bileşenleri

### UserManagementComponent
- Kullanıcı listesi tablosu
- Yeni kullanıcı ekleme formu
- Kullanıcı düzenleme formu
- Kullanıcı silme onay dialogu

### UserService
- API ile iletişimi sağlayan servis
- Token yönetimi için AuthInterceptor kullanır

## Önemli Notlar ve Çözülen Sorunlar

### API Endpoint Düzeltmesi
- Sorun: Frontend'de yanlış API endpoint'i kullanılıyordu
- Çözüm: 
  - Kullanıcı oluşturma için `/api/auth/create-user` endpoint'i kullanılmalı
  - Diğer kullanıcı işlemleri için `/api/user/...` endpoint'leri kullanılmalı

### Yetkilendirme
- Sadece admin yetkisine sahip kullanıcılar kullanıcı yönetimi sayfasına erişebilir
- AuthGuard ile sayfa erişimi kontrol edilir
- AuthInterceptor ile her isteğe JWT token eklenir

### Güvenlik
- Şifreler SHA256 ile hashlenerek saklanır
- JWT token'lar kullanıcı rollerine göre üretilir
- CORS politikası sadece frontend uygulamasına izin verir

## Test Senaryoları

1. Admin Girişi
   - Kullanıcı adı: admin
   - Şifre: admin123

2. Yeni Kullanıcı Oluşturma
   - Tüm alanlar doldurulmalı
   - Kullanıcı adı benzersiz olmalı
   - Admin yetkisi opsiyonel

3. Kullanıcı Güncelleme
   - Şifre alanı boş bırakılabilir
   - Kullanıcı adı değiştirildiğinde benzersiz olmalı

4. Kullanıcı Silme
   - Silme işlemi onay gerektirir
   - Admin kullanıcısı silinemez

## Teknik Detaylar

### Backend
- `AuthController` kullanıcı işlemlerini yönetir
- Şifre hashleme için SHA256 algoritması kullanılır (Base64 formatında)
- `StockContext` ile veritabanı işlemleri yapılır
- JWT token ile yetkilendirme kontrolü yapılır

### Frontend
- PrimeNG Table komponenti ile kullanıcı listesi görüntülenir
- Reactive Forms ile form validasyonu yapılır
- Auth Guard ile sadece admin kullanıcıların erişimi sağlanır
- Auth Interceptor ile JWT token yönetimi yapılır

## Karşılaşılan Sorunlar ve Çözümleri

### 1. Kullanıcı Yönetimi Hatası
**Sorun:** 500 Internal Server Error - Kullanıcılar yüklenirken hata oluştu

**Çözüm:**
1. AuthController'da `ApplicationDbContext` yerine `StockContext` kullanıldı
2. Şifre hash'leme yöntemi SeedData ile uyumlu hale getirildi (Base64 formatı)
3. Veritabanı sıfırlanıp yeniden oluşturuldu
4. Admin kullanıcısı doğru hash ile yeniden oluşturuldu

### 2. Şifre Hashleme Standardizasyonu
**Sorun:** Farklı yerlerde farklı hash formatları kullanılması

**Çözüm:**
1. Tüm uygulamada Base64 formatı kullanılmaya başlandı
2. SeedData ve AuthController aynı hash yöntemini kullanıyor
3. Veritabanı güncellendi ve kullanıcılar yeni formatta oluşturuldu

## Öneriler ve En İyi Uygulamalar

1. **Şifre Güvenliği**
   - SHA256 ile hashleme
   - Base64 formatında saklama
   - Minimum şifre gereksinimleri belirleme

2. **Veritabanı İşlemleri**
   - Migration'ları düzenli kullanma
   - Seed data'yı güncel tutma
   - Veritabanı değişikliklerinde dikkatli olma

3. **Kod Organizasyonu**
   - Tek bir context sınıfı kullanma
   - Hash yöntemlerini standardize etme
   - Hata mesajlarını kullanıcı dostu yapma

4. **Güvenlik**
   - Role-based access control
   - JWT token validasyonu
   - CORS politikalarını doğru yapılandırma

## Bakım ve Güncellemeler

1. **Düzenli Kontroller**
   - Kullanıcı listesinin doğru yüklenmesi
   - Yetkilendirme sisteminin çalışması
   - Hash yöntemlerinin tutarlılığı

2. **Performans İyileştirmeleri**
   - Sayfalama ve filtreleme
   - Gereksiz veritabanı sorgularını önleme
   - Önbellek kullanımı

3. **Güvenlik Güncellemeleri**
   - Şifre politikalarını güncelleme
   - Token süre kontrolü
   - Rol tabanlı erişim kontrolü

## Gelecek Geliştirmeler

1. **Planlanan Özellikler**
   - Kullanıcı rolleri yönetimi
   - Şifre sıfırlama
   - İki faktörlü doğrulama
   - Kullanıcı aktivite logları

2. **İyileştirmeler**
   - Daha detaylı hata mesajları
   - Gelişmiş filtreleme seçenekleri
   - Toplu kullanıcı işlemleri
   - Raporlama özellikleri
