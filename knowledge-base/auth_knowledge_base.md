# Kimlik Doğrulama Sistemi Geliştirme Süreci

## 1. Karşılaşılan Sorunlar ve Çözümleri

### 1.1 Veritabanı Migration Sorunları

#### Sorun 1: Dinamik Tarih Kullanımı
- **Sorun**: Seed data'da DateTime.UtcNow kullanımı her migration'da farklı değerler oluşturuyordu
- **Çözüm**: Sabit tarih kullanımı (new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc))
- **Öğrenilen Ders**: Seed data'da dinamik değerler kullanmaktan kaçınılmalı

#### Sorun 2: Database.EnsureCreated() ve Migrations Çakışması
- **Sorun**: Her iki yöntem aynı anda kullanıldığında veritabanı şeması bozuluyordu
- **Çözüm**: Database.EnsureCreated() kaldırıldı, sadece migrations kullanıldı
- **Öğrenilen Ders**: Bir projede tek bir veritabanı yönetim stratejisi kullanılmalı

### 1.2 Kullanıcı Girişi Sorunları

#### Sorun 1: 500 Internal Server Error
- **Sorun**: Login işleminde sunucu hatası
- **Çözüm**: 
  1. Detaylı loglama eklendi
  2. Try-catch blokları iyileştirildi
  3. Hata mesajları daha açıklayıcı hale getirildi
- **Öğrenilen Ders**: Hata ayıklama için detaylı loglama çok önemli

#### Sorun 2: Şifre Hash'leme
- **Sorun**: HashPassword ve ComputeSha256Hash fonksiyonları arasında tutarsızlık
- **Çözüm**: Tek bir hash fonksiyonu (ComputeSha256Hash) kullanıldı
- **Öğrenilen Ders**: Şifre işlemleri için tutarlı bir yaklaşım kullanılmalı

## 2. Geliştirme Süreci

### 2.1 Başlangıç Kurulumu
1. JWT token altyapısı kuruldu
2. PostgreSQL veritabanı bağlantısı yapılandırıldı
3. User modeli ve ApplicationDbContext oluşturuldu

### 2.2 API Geliştirme
1. AuthController oluşturuldu
2. Login ve CreateFirstAdmin endpointleri eklendi
3. JWT token üretimi için JwtService eklendi

### 2.3 Frontend Entegrasyonu
1. Login component oluşturuldu
2. AuthService ve JWT interceptor eklendi
3. Token yönetimi için localStorage kullanıldı

## 3. Test Senaryoları

### 3.1 Başarılı Durumlar
- [X] İlk admin kullanıcısı oluşturma
- [X] Admin kullanıcısı ile giriş yapma
- [X] JWT token üretimi ve doğrulama

### 3.2 Hata Durumları
- [X] Yanlış kullanıcı adı/şifre kontrolü
- [X] Boş kullanıcı adı/şifre kontrolü
- [X] Geçersiz token kontrolü

## 4. Güvenlik Önlemleri

### 4.1 Uygulanan Önlemler
1. SHA256 şifre hash'leme
2. JWT token kullanımı
3. CORS politikası
4. Input validasyonu

### 4.2 Planlanan İyileştirmeler
1. Şifre politikası güçlendirme
2. Rate limiting ekleme
3. İki faktörlü doğrulama
4. Oturum yönetimi geliştirme

## 5. Bakım ve İzleme

### 5.1 Loglama Stratejisi
1. Giriş denemeleri loglanıyor
2. Hata durumları detaylı loglanıyor
3. Hassas bilgiler maskeleniyor

### 5.2 Performans İzleme
1. Login işlem süreleri takip ediliyor
2. Veritabanı sorgu süreleri izleniyor
3. Token doğrulama süreleri ölçülüyor

# Kimlik Doğrulama Bilgi Tabanı

## Şifre Değiştirme İşlemi

### Endpoint
- `POST /api/auth/change-password`
- Yetkilendirme gerektirir (Bearer Token)

### İstek Formatı
```json
{
    "currentPassword": "string",
    "newPassword": "string"
}
```

### Başarılı Yanıt
```json
{
    "message": "Şifre başarıyla değiştirildi"
}
```

### Hata Yanıtları
- 401 Unauthorized: Token geçersiz veya eksik
- 400 Bad Request: Mevcut şifre hatalı
- 500 Internal Server Error: Sunucu hatası

### Önemli Noktalar
1. Token'dan kullanıcı ID'si alınırken `ClaimTypes.NameIdentifier` kullanılmalı
2. Şifre değiştirme işlemi öncesi mevcut şifre doğrulanmalı
3. Yeni şifre hash'lenerek veritabanına kaydedilmeli
4. İşlem detaylı şekilde loglanmalı

### Frontend Validasyonları
1. Mevcut şifre boş olmamalı
2. Yeni şifre en az 6 karakter olmalı
3. Yeni şifre tekrarı ile eşleşmeli

### Güvenlik Önlemleri
1. Şifre hash'leme için SHA256 kullanılıyor
2. Token kontrolü yapılıyor
3. Rate limiting düşünülebilir
4. Şifre politikası eklenebilir

### Loglama
```csharp
_logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
_logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
_logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
```

# Kimlik Doğrulama Sistemi Knowledge Base

## Hash Algoritması ve Şifre Yönetimi

### Önemli Noktalar
1. Hash algoritması tüm sistemde tutarlı olmalıdır
   - Seed verilerinde kullanılan hash
   - Kullanıcı girişinde kullanılan hash
   - Yeni kullanıcı oluşturmada kullanılan hash

2. Detaylı Loglama
   - Gelen şifrenin hash'i
   - Veritabanındaki hash
   - Hash algoritması detayları (SHA256, UTF8 encoding)
   - Karşılaştırma sonuçları

3. Hata Tespiti ve Çözümü
   - Hash uyumsuzluğu durumunda:
     1. Veritabanını sıfırla
     2. Doğru hash ile yeniden oluştur
     3. Migration'ları temizle
     4. Yeni migration oluştur

### Başarılı Uygulamalar
- HashService kullanımı
- Merkezi hash hesaplama
- Detaylı log kayıtları
- Tutarlı hash algoritması

### Dikkat Edilmesi Gerekenler
- Seed verilerindeki hash'lerin doğruluğu
- Hash algoritmasının tüm sistemde aynı olması
- Yeterli loglama yapılması
- Migration'ların temiz tutulması

# Yetkilendirme ve Kullanıcı Dashboard Erişimi Knowledge Base

## Genel Bakış
Bu belge, sistem giriş işlemi, kullanıcıların yönlendirilmesi ve sayfa erişim izinleriyle ilgili uygulamanın davranışını açıklar. İlgili bileşenler, karşılaşılan sorunlar ve çözümleri içerir.

## Giriş İşlemi
1. Kullanıcı, sicil numarası ve şifresini girerek `/login` sayfasından giriş yapar.
2. İstek, `AuthService` aracılığıyla backend API'sine gönderilir.
3. Backend, kullanıcıyı veritabanında arar ve şifreyi doğrular.
4. Doğrulama başarılı olursa, kullanıcı bilgileri ve izinleri içeren bir JWT token üretilir.
5. Frontend, token'ı localStorage'a kaydeder ve kullanıcı bilgilerini `currentUserSubject` üzerinden yayınlar.
6. Frontend, kullanıcı rolüne göre `/admin-dashboard` veya `/user-dashboard` sayfasına yönlendirme yapar.

## Token İçeriği
JWT token, aşağıdaki bilgileri içerir:
- `nameid`: Kullanıcı ID'si
- `unique_name`: Kullanıcı adı
- `sicil`: Kullanıcı sicil numarası
- `role`: Kullanıcı rolü ("Admin" veya "User")
- `Permission`: Kullanıcının sahip olduğu izin listesi

## Sayfa Erişim Mekanizması
1. Angular uygulamasında, `app.routes.ts` içinde rota tanımlamaları yapılır.
2. Her rota için `canActivate: [AuthGuard]` ve `data: { requiredPermission: 'PermissionName' }` şeklinde izin tanımları yapılır.
3. `AuthGuard`, `AuthService.hasPermission()` metodunu kullanarak kullanıcının ilgili sayfaya erişim iznini kontrol eder.
4. İzin yoksa, kullanıcı rolüne göre uygun sayfaya yönlendirilir.

## Karşılaşılan Zorluklar ve Çözümleri

### Sorun: Kullanıcılar Dashboard Erişimi
Standart kullanıcı rolüne sahip kullanıcılar, "Pages.UserDashboard" izni verilmediği için kullanıcı dashboard sayfasına erişemiyordu.

**Çözüm:**
- `AuthGuard` sınıfında özel bir durum eklendi: `/user-dashboard` rotası için izin kontrolü bypass edildi.
- Üç farklı yerde izin kontrolü eklemesi yapıldı:
  1. URL kontrolü
  2. Route data kontrolü
  3. PagePermissionMap kontrolü

```typescript
// Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
if (url === '/user-dashboard') {
  return true;
}

// ...

// Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
if (requiredPermission === 'Pages.UserDashboard') {
  return true;
}

// ...

// Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
if (pagePermission === 'Pages.UserDashboard') {
  return true;
}
```

### Sorun: Token'dan İzinlerin Yüklenmesi
Kullanıcı giriş yaptığında token'dan izinlerin yüklenmesi unutulmuştu.

**Çözüm:**
- `AuthService.login()` metoduna izinleri yükleme kodu eklendi:
```typescript
// İzinleri yükle
this.permissions = decodedToken.Permission || [];
```

## İleride Yapılabilecek İyileştirmeler
1. Backend'de standart kullanıcı rolüne "Pages.UserDashboard" izninin eklenmesi
2. `IPermissionService.GetPermissionsByRoleIdAsync()` metodunda performans iyileştirmesi
3. Frontend'de token yenilemesi (refresh token) mekanizmasının eklenmesi
4. İzinlerin cache'lenmesi ve daha etkin kullanımı
5. Sayfa yükleniyor göstergesi eklenmesi

## Önerilen İzin Yapısı
- Admin rolü: Tüm izinlere sahip
- User rolü: En azından aşağıdaki izinlere sahip olmalı
  - Pages.UserDashboard
  - Dashboard.View
  - Pages.BilgiIslem.View (kullanıcının BİLGİ İŞLEM modülüne erişebilmesi için)
  - Stock.Read
